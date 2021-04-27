using MongoBase.Attributes;
using MongoBase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoBase.Test.Models
{
    [MongoBase.Attributes.BsonCollection("TestModel")]
   public class TestModel:MongoBase.Models.BaseDocument,IFeedDocument
    {
        [IsQueryableAttribute()]
        public string Name { get; set; }
        [IsQueryableAttribute()]
        public string Instance { get; set; }
        [IsQueryableAttribute()]
        public string Context { get; set; }
        [IsQueryableAttribute()]
        public long Timestamp { get; set; }
    }
}
