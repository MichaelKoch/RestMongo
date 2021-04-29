using RestMongo.Attributes;
using RestMongo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestMongo.Test.Models
{
    [RestMongo.Attributes.BsonCollection("TestModelFeed")]
    public class TestModelFeed : RestMongo.Models.FeedDocument
    {
        [IsQueryableAttribute()]
        public string Name { get; set; }
        [IsQueryableAttribute()]
        public string Instance { get; set; }
        [IsQueryableAttribute()]
        public string Context { get; set; }

        public string NotQueryable { get; set; }




    }
}
