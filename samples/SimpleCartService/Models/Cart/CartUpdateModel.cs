using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SimpleCartService.Models.Cart
{
    public class CartUpdateModel
    {
        [JsonPropertyName("ExternalId")]
        public string ExternalId { get; set; }
        [JsonPropertyName("CustomerId")]
        [Required]
        public string CustomerId { get; set; }
    }
}
