using Cassandra;
using Newtonsoft.Json;
using DataStax.AstraDB.DataApi.Tables;

namespace kv_be_csharp_dataapi_table.Models;

public class UserComment
{
    [ColumnPrimaryKey(1)]
    [ColumnName("userid")]
    public Guid userid { get; set; } = Guid.Empty;

    [ColumnPrimaryKey(2)]
    [ColumnName("commentid")]
    public TimeUuid commentid { get; set; } = TimeUuid.NewId();
    public string comment { get; set; } = string.Empty;
    public Guid videoid { get; set; } = Guid.Empty;

    [ColumnName("sentiment_score")]
    [JsonProperty("sentiment_score")]
    public float sentimentScore { get; set; } = 0.0F;

    public static UserComment fromComment(Comment comment)
    {
        UserComment userComment = new();

        userComment.comment = comment.comment;
        userComment.commentid = comment.commentid;
        userComment.sentimentScore = comment.sentimentScore;
        userComment.userid = comment.userid;
        userComment.videoid = comment.videoid;

        return userComment;
    }
}