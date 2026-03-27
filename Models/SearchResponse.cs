namespace kv_be_csharp_dataapi_table.Models;

public class SearchResponse
{
    public List<VideoResponse> data { get; set; } = new();
    public Pagination pagination { get; set; }

    public SearchResponse()
    {
        this.pagination = new Pagination(1, 1, 0, 0);
    }

    public SearchResponse(List<VideoResponse> data, int currentPage, int pageSize, int totalItems)
    {
        this.data = data;
        this.pagination = new Pagination(currentPage, pageSize, totalItems);
    }
}

// Made with Bob
