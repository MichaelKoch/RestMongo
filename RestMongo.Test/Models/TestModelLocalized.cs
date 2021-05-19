using System.Text.Json.Serialization;
using RestMongo.Data.Attributes;
using RestMongo.Data.Repository.Documents;

namespace RestMongo.Test.Models
{
    [BsonCollection("TestModelLocalized")]
    public class TestModelLocalized : LocalizedDocument
    {
        [IsQueryableAttribute()]
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [IsQueryableAttribute()]
        [JsonPropertyName("Instance")]
        public string Instance { get; set; }
        [IsQueryableAttribute()]
        [JsonPropertyName("Context")]
        public string Context { get; set; }

        public string NotQueryable { get; set; }

        [IsQueryableAttribute()]
        public override string Locale { get => base.Locale; set => base.Locale = value; }





    }
}
