using DataStax.AstraDB.DataApi.Core;
using kv_be_csharp_dataapi_table.Models;

namespace kv_be_csharp_dataapi_table.Repositories;

public class VideoDAL : IVideoDAL
{
    private readonly Database _database;

    public VideoDAL(ICassandraConnection cassandraConnection)
    {        
        _database = cassandraConnection.GetDatabase();
    }

    public Video SaveVideo(Video video)
    {
        var table = _database.GetTable<Video>("videos");
        
        table.InsertOneAsync(video);

        return video;
    }
    
    public async Task UpdateVideo(Video video)
    {
        var table = _database.GetTable<Video>("videos");

        var filter = Builders<Video>.Filter.Eq(v => v.videoId, video.videoId);

        var update = Builders<Video>.Update
            .Set(v => v.addedDate, video.addedDate)
            .Set(v => v.category, video.category)
            .Set(v => v.contentFeatures, video.contentFeatures)
            .Set(v => v.contentFeatures, video.contentFeatures)
            .Set(v => v.description, video.description)
            .Set(v => v.language, video.language)
            .Set(v => v.location, video.location)
            .Set(v => v.locationType, video.locationType)
            .Set(v => v.name, video.name)
            .Set(v => v.previewImageLocation, video.previewImageLocation)
            .Set(v => v.tags, video.tags)
            .Set(v => v.userId, video.userId)
            .Set(v => v.views, video.views)
            .Set(v => v.youtubeId, video.youtubeId);

        await table.UpdateOneAsync(filter, update);
    }

    public async Task UpdateVideoView(Video video)
    {
        var table = _database.GetTable<Video>("videos");

        var filter = Builders<Video>.Filter.Eq(v => v.videoId, video.videoId);

        var update = Builders<Video>.Update
            .Set(v => v.views, video.views);

        await table.UpdateOneAsync(filter, update);
    }

    public async Task UpdateVideoPlaybackStats(VideoPlaybackStats video)
    {
        var table = _database.GetTable<VideoPlaybackStats>("video_playback_stats");

        var filter = Builders<VideoPlaybackStats>.Filter.Eq(v => v.videoId, video.videoId);

        var update = Builders<VideoPlaybackStats>.Update
            .Set(v => v.completeViews, video.completeViews)
            .Set(v => v.totalPlayTime, video.totalPlayTime)
            .Set(v => v.uniqueViewers, video.uniqueViewers)
            .Set(v => v.views, video.views);

        await table.UpdateOneAsync(filter, update);
    }

    public async Task InsertVideoActivity(VideoActivity video)
    {
        var table = _database.GetTable<VideoActivity>("video_activity");
        
        await table.InsertOneAsync(video);
    }

    public async Task<Video?> GetVideoByVideoId(Guid videoId)
    {
        var table = _database.GetTable<Video>("videos");

        var filterBuilder = Builders<Video>.Filter;
        var filter = filterBuilder.Eq("videoid", videoId);

        Video video = await table.FindOneAsync(filter);

        return video;
    }

    public async Task<VideoPlaybackStats?> GetVideoPlaybackStatsByVideoId(Guid videoId)
    {
        var table = _database.GetTable<VideoPlaybackStats>("video_playback_stats");

        var filterBuilder = Builders<VideoPlaybackStats>.Filter;
        var filter = filterBuilder.Eq("videoid", videoId);

        return await table.FindOneAsync(filter);
    }

    public async Task<IEnumerable<VideoActivity>> GetVideoActivity(DateOnly day)
    {
        var table = _database.GetTable<VideoActivity>("video_activity");

        var filterBuilder = Builders<VideoActivity>.Filter;
        var filter = filterBuilder.Eq("day", day);

        return table.Find(filter);
    }

    public async Task<IEnumerable<Video>> GetByVector(float[]? vector, int limit)
    {
        var table = _database.GetTable<Video>("videos");

        var vectorSearchData = table.Find().Sort(
            Builders<Video>.TableSort.Vector(
            v => v.contentFeatures,
            vector
            )
        ).Limit(limit);

        return vectorSearchData;
    }
}