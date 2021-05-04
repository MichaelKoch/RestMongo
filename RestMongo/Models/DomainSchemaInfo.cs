using System.Text.Json.Serialization;

namespace RestMongo.Models
{
    [RestMongo.Attributes.BsonCollection("schema.version")]
    public class DomainSchemaInfo : BaseDocument
    {
        [JsonPropertyName("AssemblyFullName")]
        public string AssemblyName { get; set; }

        [JsonPropertyName("AssemblyVersion")]
        public string AssemblyVersion { get; set; }

        [JsonPropertyName("AppliedSchema")]
        public string AppliedSchema { get; set; }

        [JsonPropertyName("Hash")]
        public string Hash { get; set; }
    }
}
