using MongoBase.Attributes;
using MongoBase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoBase.Test.Models
{
    [MongoBase.Attributes.BsonCollection("TestModelFeedLocalized")]
   public class TestModelFeedLocalized : MongoBase.Models.LocalizedFeedDocument
    {
        [IsQueryableAttribute()]
        public string Name { get; set; }
        [IsQueryableAttribute()]
        public string Instance { get; set; }
        [IsQueryableAttribute()]
        public string Context { get; set; }

        public string NotQueryable { get; set; }
        [IsQueryableAttribute()]
        public override string Locale { get => base.Locale; set => base.Locale = value; }




    }
}
