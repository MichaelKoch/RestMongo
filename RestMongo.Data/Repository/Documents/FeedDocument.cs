using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using RestMongo.Data.Abstractions.Repository.Mongo.Documents;
using RestMongo.Data.Attributes;

namespace RestMongo.Data.Repository.Documents
{
    [BsonIgnoreExtraElements]
    public class FeedDocument : BaseDocument, IFeedDocument
    {
        [IsQueryable]
        [JsonPropertyName("Timestamp")]
        public long Timestamp { get; set; }
    }
}