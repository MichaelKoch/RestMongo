using System.Text.Json.Serialization;
using RestMongo.Data.Attributes;
using RestMongo.Data.Repository.Documents;

namespace RestMongo.Test.Models
{
    [BsonCollection("TestModel")]
    public class TestModel : BaseDocument
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
    }
}
