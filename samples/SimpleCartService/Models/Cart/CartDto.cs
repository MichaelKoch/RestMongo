using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using RestMongo.Data.Attributes;
using SimpleCartService.Models.CartItem;

namespace SimpleCartService.Models.Cart
{
    public record CartDto
    {
        [IsQueryable()]
        [JsonPropertyName("Id")]
        [Required]
        public string Id { get; set; }

        [IsQueryable()]
        [JsonPropertyName("ExternalId")]
        public string ExternalId { get; set; }

        [IsQueryable()]
        [JsonPropertyName("CustomerId")]
        [Required]
        public string CustomerId { get; set; }

        [JsonPropertyName("Items")] public List<CartItemCreateModel> Items { get; set; }
    }
}