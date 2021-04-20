using System.Text.Json.Serialization;
using MongoBase.Attributes;

[MongoBase.Attributes.BsonCollection("MaterialClassification")]
public class MaterialClassification : MongoBase.Models.BaseDocument
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
    [JsonPropertyName("AttributeId")]
    public string AttributeId { get; set; }
    [IsQueryableAttribute()]
    [JsonPropertyName("Label")]
    public string Label { get; set; }
    [IsQueryableAttribute()]
    [JsonPropertyName("Value")]
    public string Value { get; set; }
    [IsQueryableAttribute()]
    [JsonPropertyName("ValueText")]
    public string ValueText { get; set; }
}