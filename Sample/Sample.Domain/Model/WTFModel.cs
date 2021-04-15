using MongoBase.Attributes;
using System.Text.Json.Serialization;

namespace Sample.Domain.Models
{

    [MongoBase.Attributes.BsonCollection("WTF")]
    public class WTFModel : MongoBase.Models.BaseDocument
    {

        [SchemaAttribute(isSimple: true)]
        [JsonPropertyName("RetailProductGroup")]
        public string Name { get; set; }
    }
}