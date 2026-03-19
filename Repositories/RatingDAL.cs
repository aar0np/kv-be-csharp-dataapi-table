using kv_be_csharp_dataapi_table.Models;

namespace kv_be_csharp_dataapi_table.Repositories;

public class RatingDAL : IRatingDAL
{
    private readonly Cassandra.ISession _session;
    private readonly IMapper _mapper;

    public RatingDAL(ICassandraConnection cassandraConnection)
    {
        _session = cassandraConnection.GetCQLSession();
        _mapper = new Mapper(_session);
    }

    public async Task<IEnumerable<Rating?>> FindByVideoId(Guid videoid)
    {
        return await _mapper.FetchAsync<Rating>("WHERE videoid=?", videoid);
    }

    public async Task<Rating?> FindByVideoIdAndUserId(Guid videoid, Guid userid)
    {
        return await _mapper.FirstOrDefaultAsync<Rating>("WHERE videoid=? AND userid=?", videoid, userid);
    }

    public async Task<Rating> SaveRating(Rating rating)
    {
        await _mapper.InsertAsync<Rating>(rating);
        return rating;
    }

    public async Task Update(Rating rating)
    {
        await _mapper.UpdateAsync<Rating>(rating);
    }
}