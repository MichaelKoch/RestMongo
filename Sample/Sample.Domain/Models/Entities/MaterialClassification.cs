using System.Text.Json.Serialization;
using RestMongo.Attributes;
using RestMongo.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using Sample.Domain.Models.Enities;

[RestMongo.Attributes.BsonCollection("MaterialClassification")]
[BsonIgnoreExtraElements]
public class MaterialClassification : RestMongo.Models.BaseDocument, IFeedDocument
{
    [IsQueryableAttribute()]
    [JsonPropertyName("Id")]
    public override string Id
    {
        get { return this.MaterialNumber + "-" + this.AttributeId + "-" + this.Value + "-" + this.Locale; }
        set { }
    }


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

    [IsQueryableAttribute()]
    [JsonPropertyName("Timestamp")]
    public long Timestamp { get; set; }



    public CollectionMaterialDTO Material { get; set; }
}