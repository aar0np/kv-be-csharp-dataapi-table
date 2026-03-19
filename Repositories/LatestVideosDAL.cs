using kv_be_csharp_dataapi_table.Models;

namespace kv_be_csharp_dataapi_table.Repositories;

public class LatestVideosDAL : ILatestVideosDAL
{
    private readonly Cassandra.ISession _session;
    private readonly IMapper _mapper;

    public LatestVideosDAL(ICassandraConnection cassandraConnection)
    {
        _session = cassandraConnection.GetCQLSession();
        _mapper = new Mapper(_session);
    }

    public LatestVideo SaveLatestVideo(LatestVideo video)
    {
        _mapper.Insert(video);

        return video;
    }

    public async Task<IEnumerable<LatestVideo>> GetLatestVideosToday(LocalDate day, int limit)
    {
        var latestVideosData =
            await _mapper.FetchAsync<LatestVideo>("WHERE day=? LIMIT ?", day, limit);

        return latestVideosData;
    }

    // SELECTs FROM latest_videos LIMIT limit; try not to use
    public async Task<IEnumerable<LatestVideo>> GetLatestVideos(int limit)
    {
        var latestVideosData =
            await _mapper.FetchAsync<LatestVideo>("LIMIT ?", limit);

        return latestVideosData;
    }
}