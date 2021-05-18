using System.Text.Json.Serialization;

namespace SimpleCartService.Models.CartItem
{
    public class CartItemCreateModel
    {


        [JsonPropertyName("ArticleId")]
        public string ArticleId { get; set; }


        [JsonPropertyName("Quantity")]
        public int Quantity { get; set; }

    }
}
