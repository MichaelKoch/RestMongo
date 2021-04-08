using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MongoBase.Attributes;

namespace SampleServer.Models
{
    [MongoBase.Attributes.BsonCollection("TestModel")]
    public class TestModel : MongoBase.Models.BaseDocument
    {
        [SchemaAttribute(isSimple: true)]
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [SchemaAttribute(isSimple: true)]
        [JsonPropertyName("Instance")]
        public long Instance { get; set; }


    }
}
