using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
