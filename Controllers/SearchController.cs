using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

using kv_be_csharp_dataapi_table.Models;
using kv_be_csharp_dataapi_table.Repositories;

namespace kv_be_csharp_dataapi_table.Controllers;

[ApiController]
[Route("/api/v1/search")]
[Produces("application/json")]
public class SearchController : Controller
{
    private string? _HF_API_KEY = System.Environment.GetEnvironmentVariable("HF_API_KEY");
    private static readonly string _modelId = "ibm-granite/granite-embedding-30m-english";
    private static readonly string _HF_APLOETZ_SPACE_ENDPOINT = "https://aploetz-granite-embeddings.hf.space/embed";
    private HttpClient _hFhttpClient;

    private readonly IVideoDAL _videoDAL;
    private readonly IRatingDAL _ratingDAL;

    public SearchController(IVideoDAL videoDAL, IRatingDAL ratingDAL)
    {
        _videoDAL = videoDAL;
        _ratingDAL = ratingDAL;

        // check HuggingFace API KEY from env var
        if (string.IsNullOrEmpty(_HF_API_KEY))
        {
            Console.WriteLine("ERROR: HF_API_KEY must be defined as an environment variable.");
        }

        // define HTTP client to hit HuggingFace embedding model
        _hFhttpClient = new HttpClient();
    }

    [HttpGet("videos")]
    [ProducesResponseType(typeof(SearchResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SearchResponse>> SearchVideos(
        [FromQuery] string query,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            // Validate parameters
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query parameter is required");
            }

            if (page <= 0)
            {
                page = 1;
            }

            if (pageSize <= 0 || pageSize > 100)
            {
                pageSize = 10;
            }

            // Generate the embedding for the search query
            var req = new HuggingFaceRequest();
            req.text = query;
            req.model = _modelId;

            var json = JsonConvert.SerializeObject(req);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var hFRequestMessage = new HttpRequestMessage(HttpMethod.Post, _HF_APLOETZ_SPACE_ENDPOINT)
            {
                Content = data
            };
            
            HttpResponseMessage hFResponse = await _hFhttpClient.SendAsync(hFRequestMessage);
            
            if (!hFResponse.IsSuccessStatusCode)
            {
                Console.WriteLine("Error generating embedding: " + hFResponse.StatusCode);
                return BadRequest("Error generating search embedding");
            }

            string jsonResponse = await hFResponse.Content.ReadAsStringAsync();
            HuggingFaceResponse hFResp = JsonConvert.DeserializeObject<HuggingFaceResponse>(jsonResponse);

            if (hFResp == null || hFResp.embedding == null || hFResp.embedding.Length == 0)
            {
                Console.WriteLine("Invalid embedding response");
                return BadRequest("Invalid embedding response");
            }

            // Calculate limit for vector search (get more than needed for pagination)
            int limit = page * pageSize;

            // Search videos using the embedding
            var videos = await _videoDAL.GetByVector(hFResp.embedding, limit);
            var videosList = videos.ToList();

            // Calculate pagination
            int totalItems = videosList.Count;
            int startIndex = (page - 1) * pageSize;
            int endIndex = Math.Min(startIndex + pageSize, totalItems);

            // Get the page of results
            List<VideoResponse> response = new();

            for (int i = startIndex; i < endIndex; i++)
            {
                var video = videosList[i];
                VideoResponse videoResponse = VideoResponse.fromVideo(video);

                // Get rating information
                var rating = await _ratingDAL.FindByVideoId(video.videoId);

                if (rating is not null)
                {
                    int ratingCount = rating.ratingCounter;
                    int totalRating = rating.ratingTotal;

                    if (ratingCount > 0)
                    {
                        videoResponse.averageRating = totalRating / ratingCount;
                    }
                    else
                    {
                        videoResponse.averageRating = 0.0f;
                    }
                }
                else
                {
                    videoResponse.averageRating = 0.0f;
                }

                response.Add(videoResponse);
            }

            // Create search response with pagination
            SearchResponse searchResponse = new SearchResponse(response, page, pageSize, totalItems);

            Console.WriteLine($"Search completed for query: '{query}', found {totalItems} results");
            return Ok(searchResponse);

        }
        catch (Exception e)
        {
            Console.WriteLine("Error processing search request: " + e.Message);
            return BadRequest("Error processing search request");
        }
    }
}

// Made with Bob
