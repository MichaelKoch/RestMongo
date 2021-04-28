using System.Text.Json.Serialization;
using MongoBase.Attributes;
using MongoBase.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using Sample.Domain.Models.Enities;

[MongoBase.Attributes.BsonCollection("MaterialText")]
[BsonIgnoreExtraElements]
public class MaterialText : MongoBase.Models.BaseDocument,IFeedDocument
{
    [IsQueryableAttribute()]
    [JsonPropertyName("Id")]
    public override string Id { 
        get 
        {
            return this.MaterialNumber + "-" + Locale;
        } 
        set { } }
  

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
    [IsQueryableAttribute()]
    [JsonPropertyName("Timestamp")]
    public long Timestamp { get; set; }


    public CollectionMaterialDTO Material { get; set;}
}