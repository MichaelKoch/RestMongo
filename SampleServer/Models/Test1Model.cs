using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleServer.Models
{
    [MongoBase.Attributes.BsonCollection("Test1Model")]
   public class Test1Model:MongoBase.Models.BaseDocument
    {
        public string Name { get; set; }
        public long Instance { get; set; }
    }
}
