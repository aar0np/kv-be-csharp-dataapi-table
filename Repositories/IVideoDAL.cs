using kv_be_csharp_dataapi_table.Models;

namespace kv_be_csharp_dataapi_table.Repositories;
public interface IVideoDAL
{
    Task<Video?> GetVideoByVideoId(Guid videoId);

    Video SaveVideo(Video video);

    void UpdateVideo(Video video);

    Task<IEnumerable<Video>> GetByVector(CqlVector<float> vector, int limit);
}