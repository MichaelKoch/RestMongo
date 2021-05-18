using System.Text.Json.Serialization;
using RestMongo.Data.Attributes;
using RestMongo.Data.Repository.Documents;

namespace RestMongo.Test.Models
{
    [BsonCollection("schema.version")]
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
