using System.Text.Json.Serialization;

namespace RestMongo.Data.Abstractions.Repository.Mongo.Documents
{
    public interface IFeedDocument : IDocument
    {
        [JsonPropertyName("Timestamp")]
        public long Timestamp { get; set; }
    }
}
