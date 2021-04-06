using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoBase.Test.Models
{
    [MongoBase.Attributes.BsonCollection("TestModel")]
   public class TestModel:MongoBase.Models.BaseDocument
    {
        public string Name { get; set; }
        public long Instance { get; set; }
    }
}
