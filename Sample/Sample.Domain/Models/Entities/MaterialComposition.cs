
using System.Text.Json.Serialization;
using MongoBase.Attributes;
using MongoBase.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using Sample.Domain.Models.Enities;

[MongoBase.Attributes.BsonCollection("MaterialComposition")]
[BsonIgnoreExtraElements]
public class MaterialComposition : MongoBase.Models.BaseDocument,IFeedDocument
{
    [IsQueryableAttribute()]
    [JsonPropertyName("Id")]
    public override string Id { 
            get { return this.MaterialNumber + "-" + this.Component +"-" + this.MaterialCompositionID  +"-" + this.Locale; } set { } }


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

    [IsQueryableAttribute()]
    [JsonPropertyName("Timestamp")]
    public long Timestamp { get; set; }





}

