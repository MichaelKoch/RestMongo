using System.Text.Json.Serialization;
using RestMongo.Data.Attributes;
using RestMongo.Data.Repository.Documents;

namespace RestMongo.Test.Models
{
    public class TestModelFeed : FeedDocument
    {
        [IsQueryable()]
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [IsQueryableAttribute()]
        [JsonPropertyName("Instance")]
        public string Instance { get; set; }

        [IsQueryableAttribute()]
        [JsonPropertyName("Context")]
        public string Context { get; set; }

        public string NotQueryable { get; set; }

        public override TTarget Transform<TTarget>()
        {
            return base.Transform<TTarget>();
        }
    }
}