using MongoDB.Bson.Serialization.Attributes;
using RestMongo.Attributes;
using RestMongo.Interfaces;
using System.Text.Json.Serialization;

namespace RestMongo.Models
{
    [BsonIgnoreExtraElements]
    public class FeedDocument : BaseDocument, IFeedDocument
    {
        [IsQueryableAttribute()]
        [JsonPropertyName("Timestamp")]
        public long Timestamp { get; set; }
    }
}