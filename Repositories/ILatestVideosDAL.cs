using kv_be_csharp_dataapi_table.Models;

namespace kv_be_csharp_dataapi_table.Repositories;

public interface ILatestVideosDAL
{
    LatestVideo SaveLatestVideo(LatestVideo video);

    Task<IEnumerable<LatestVideo>> GetLatestVideosToday(LocalDate day, int limit);
    Task<IEnumerable<LatestVideo>> GetLatestVideos(int limit);
}