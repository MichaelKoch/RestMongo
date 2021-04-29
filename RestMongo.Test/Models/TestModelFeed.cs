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
    public class TestModelFeed : RestMongo.Models.FeedDocument,ITransformable
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
       
        public override TTarget Transform<TTarget>()
        {
            return base.Transform<TTarget>();
        }



    }
}
