namespace kv_be_csharp_dataapi_table.Models;

public class LatestVideosResponse
{
    public List<VideoResponse> data { get; set; }

    public LatestVideosResponse(List<VideoResponse> videoResponses)
    {
        data = videoResponses;
    }
}