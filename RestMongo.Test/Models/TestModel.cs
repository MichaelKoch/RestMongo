using RestMongo.Attributes;
using RestMongo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestMongo.Test.Models
{
    [RestMongo.Attributes.BsonCollection("TestModel")]
    public class TestModel : RestMongo.Models.BaseDocument
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


    }
}
