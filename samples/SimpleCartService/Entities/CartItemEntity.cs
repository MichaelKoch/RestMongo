using System.Text.Json.Serialization;
using RestMongo.Data.Attributes;
using RestMongo.Data.Repository.Documents;

namespace SimpleCartService.Entities
{
    public class CartItemEntity : FeedDocument
    {
        [IsQueryable()]
        [JsonPropertyName("CartId")]
        public string CartId { get; set; }

        [IsQueryableAttribute()]
        [JsonPropertyName("ArticleId")]
        public string ArticleId { get; set; }

        [IsQueryableAttribute()]
        [JsonPropertyName("Quantity")]
        public decimal Quantity { get; set; }
    }
}
