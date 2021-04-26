using System.Text.Json.Serialization;
using MongoBase.Attributes;
using MongoBase.Interfaces;
using Sample.Domain.Models.Enities;


public class MaterialTextDTO 
{
    
    [JsonPropertyName("DisplayText")]
    public string DisplayText { get; set; }
    
    [JsonPropertyName("DetailText")]
    public string DetailText { get; set; }
    
    [JsonPropertyName("Description")]
    public string Description { get; set; }
    
}