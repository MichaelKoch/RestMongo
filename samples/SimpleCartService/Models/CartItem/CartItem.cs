using System.Text.Json.Serialization;

namespace SimpleCartService.Models.CartItem
{
    public class CartItem
    {
        [JsonPropertyName("Id")] public string Id { get; set; }
        [JsonPropertyName("CartId")] public string CartId { get; set; }
        [JsonPropertyName("ArticleId")] public string ArticleId { get; set; }
        [JsonPropertyName("Quantity")] public int Quantity { get; set; }
    }
}