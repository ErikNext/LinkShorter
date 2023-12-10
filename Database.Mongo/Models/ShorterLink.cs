using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Database.Mongo.Models;

public class ShorterLink
{
    [BsonId]
    public string Id { get; set; }
    public string Key { get; set; }
    public string MainLink { get; set; }
    public int TransitionsCount { get; set; }

    public ShorterLink(string id, string key, string mainLink, int transitionsCount = 0)
    {
        Key = key;
        MainLink = mainLink;
        Id = id;
        TransitionsCount = transitionsCount;
    }
}