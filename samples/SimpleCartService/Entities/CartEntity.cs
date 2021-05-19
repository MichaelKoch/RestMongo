using System.Text.Json.Serialization;
using RestMongo.Data.Attributes;
using RestMongo.Data.Repository.Documents;

namespace SimpleCartService.Entities
{
    public class CartEntity : FeedDocument
    {
        //force clean (uri) free id's  // catch mongo id exceptions
        [IsQueryable()]
        [JsonPropertyName("ExternalId")]
        public string ExternalId { get; set; }

        [IsQueryableAttribute()]
        [JsonPropertyName("CartId")]
        public string CartId { get; set; }

        [IsQueryableAttribute()]
        [JsonPropertyName("CustomerId")]
        public string CustomerId { get; set; }
    }
}
