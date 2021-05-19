using System.Text.Json.Serialization;

namespace SimpleCartService.Models.CartItem
{
    public class CartItemUpdateModel
    {


        [JsonPropertyName("ArticleId")]
        public string ArticleId { get; set; }


        [JsonPropertyName("Quantity")]
        public decimal Quantity { get; set; }

    }
}
