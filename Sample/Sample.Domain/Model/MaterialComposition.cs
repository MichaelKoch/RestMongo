
using System.Text.Json.Serialization;
using MongoBase.Attributes;

[MongoBase.Attributes.BsonCollection("MaterialComposition")]
public class MaterialComposition : MongoBase.Models.BaseDocument
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
    [JsonPropertyName("MaterialCompositionID")]
    public string MaterialCompositionID { get; set; }
    
    [IsQueryableAttribute()]
    [JsonPropertyName("Component")]
    public string Component { get; set; }
    
    [IsQueryableAttribute()]
    [JsonPropertyName("ComponentText")]
    public string ComponentText { get; set; }
    [IsQueryableAttribute()]
    [JsonPropertyName("Proportion")]
    public int Proportion { get; set; }
    
    [IsQueryableAttribute()]
    [JsonPropertyName("Text")]
    public string Text { get; set; }
    
    [IsQueryableAttribute()]
    [JsonPropertyName("Abbreviation")]
    public string Abbreviation { get; set; }
}

