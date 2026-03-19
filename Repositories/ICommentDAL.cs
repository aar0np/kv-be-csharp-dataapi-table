using Cassandra;
using kv_be_csharp_dataapi_table.Models;

namespace kv_be_csharp_dataapi_table.Repositories;

public interface ICommentDAL
{
    Comment SaveComment(Comment comment);
    Task<Comment> UpdateComment(Comment comment);
    UserComment SaveUserComment(UserComment comment);
    Task<UserComment> UpdateUserComment(UserComment comment);
    Task<Comment?> GetCommentById(TimeUuid commentid);
    Task<IEnumerable<Comment?>> GetCommentsByVideoId(Guid videoId, int limit);
    Task<IEnumerable<UserComment?>> GetCommentsByUserId(Guid userId);
    Task DeleteComment(Guid videoid, TimeUuid commentid);
    Task DeleteUserComment(Guid userid, TimeUuid commentid);
}