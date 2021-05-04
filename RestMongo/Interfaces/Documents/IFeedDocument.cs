using RestMongo.Attributes;
using System.Text.Json.Serialization;

namespace RestMongo.Interfaces
{
    public interface IFeedDocument : IDocument
    {
        [IsQueryableAttribute()]
        [JsonPropertyName("Timestamp")]
        public long Timestamp { get; set; }


    }
}
