using System.Management;
using System.Threading.Tasks;
using Cassandra;
using DataStax.AstraDB.DataApi.Core;
using DataStax.AstraDB.DataApi.Core.Query;
using DataStax.AstraDB.DataApi.Tables;

using kv_be_csharp_dataapi_table.Models;
using tryAGI.OpenAI;

namespace kv_be_csharp_dataapi_table.Repositories;

public class CommentDAL : ICommentDAL
{
    private readonly Database _database;

    public CommentDAL(ICassandraConnection cassandraConnection)
    {
        _database = cassandraConnection.GetDatabase();
    }

    public async Task DeleteComment(Guid videoid, TimeUuid commentid)
    {
        var table = _database.GetTable<Comment>("comments");

        // Delete a row
        var filterBuilder = Builders<Comment>.Filter;
        Filter<Comment> filter = filterBuilder.And(
            filterBuilder.Eq("videoid", videoid),
            filterBuilder.Eq("commentid", commentid)
        );

        await table.DeleteOneAsync(filter);
    }

    public async Task DeleteUserComment(Guid userid, TimeUuid commentid)
    {
        var table = _database.GetTable<Comment>("comments_by_user");

        // Delete a row
        var filterBuilder = Builders<Comment>.Filter;
        var filter = filterBuilder.And(
            filterBuilder.Eq("userid", userid),
            filterBuilder.Eq("commentid", commentid)
        );

        await table.DeleteOneAsync(filter);
    }

    public async Task<Comment?> GetCommentById(TimeUuid commentid)
    {
        var table = _database.GetTable<Comment>("comments");

        var filterBuilder = Builders<Comment>.Filter;
        var filter = filterBuilder.Eq("commentid", commentid);

        Comment comment = await table.FindOneAsync(filter);
        
        return comment;
    }

    public async Task<IEnumerable<UserComment?>> GetCommentsByUserId(Guid userId)
    {
        var table = _database.GetTable<UserComment>("comments_by_user");

        var filterBuilder = Builders<UserComment>.Filter;
        var filter = filterBuilder.Eq("userid", userId);

        var comments = table.Find(filter);
        //var comments = await table.FindAsync(filter);
        
        return comments;
    }

    public async Task<IEnumerable<Comment?>> GetCommentsByVideoId(Guid videoId, int limit)
    {
        var table = _database.GetTable<Comment>("comments");

        var filterBuilder = Builders<Comment>.Filter;
        var filter = filterBuilder.Eq("videoid", videoId);

        var comments = table.Find(filter).Limit(limit);
        
        return comments;
    }

    public Comment SaveComment(Comment comment)
    {
        var table = _database.GetTable<Comment>("comments");

        table.InsertOneAsync(comment);

        return comment;
    }

    public async Task<Comment> UpdateComment(Comment comment)
    {
        var table = _database.GetTable<Comment>("comments");

        var filter = Builders<Comment>.Filter.CompositeKey(
            new PrimaryKeyFilter<Comment, Guid>(c => c.videoid, comment.videoid),
            new PrimaryKeyFilter<Comment, TimeUuid>(c => c.commentid, comment.commentid)
        );

        var update = Builders<Comment>.Update
            .Set(c => c.comment, comment.comment)
            .Set(c => c.sentimentScore, comment.sentimentScore);

        await table.UpdateOneAsync(filter, update);

        return comment;
    }

    public UserComment SaveUserComment(UserComment comment)
    {
        var table = _database.GetTable<UserComment>("comments_by_user");

        table.InsertOneAsync(comment);

        return comment;
    }
    
    public async Task<UserComment> UpdateUserComment(UserComment comment)
    {
        var table = _database.GetTable<UserComment>("comments_by_user");

        var filter = Builders<UserComment>.Filter.CompositeKey(
            new PrimaryKeyFilter<UserComment, Guid>(c => c.userid, comment.userid),
            new PrimaryKeyFilter<UserComment, TimeUuid>(c => c.commentid, comment.commentid)
        );

        var update = Builders<UserComment>.Update
            .Set(c => c.comment, comment.comment)
            .Set(c => c.sentimentScore, comment.sentimentScore);

        await table.UpdateOneAsync(filter, update);

        return comment;
    }
}