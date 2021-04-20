using System.Text.Json.Serialization;
using MongoBase.Attributes;

[MongoBase.Attributes.BsonCollection("MaterialText")]
public class MaterialText : MongoBase.Models.BaseDocument
{
    [IsQueryableAttribute()]
    [JsonPropertyName("Id")]
    public override string Id { get; set; }
    [IsQueryableAttribute()]
    [JsonPropertyName("ChangedAt")]
    public override long ChangedAt { get; set; }
    
    [IsQueryableAttribute()]
    [JsonPropertyName("MaterialNumber")]
    public int MaterialNumber { get; set; }
    
    [IsQueryableAttribute()]
    [JsonPropertyName("Locale")]
    public string Locale { get; set; }
    
    [IsQueryableAttribute()]
    [JsonPropertyName("DisplayText")]
    public string DisplayText { get; set; }
    
    [IsQueryableAttribute()]
    [JsonPropertyName("DetailText")]
    public string DetailText { get; set; }
    
    [IsQueryableAttribute()]
    [JsonPropertyName("Description")]
    public string Description { get; set; }
}