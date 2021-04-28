using MongoBase.Attributes;
using MongoBase.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sample.Domain.Models.Enities
{
    [MongoBase.Attributes.BsonCollection("CollectionMaterial")]
    [BsonIgnoreExtraElements]
    public class CollectionMaterial : MongoBase.Models.LocalizedFeedDocument, ILocalizedFeedDocument
    {



        [IsQueryableAttribute()]
        [JsonPropertyName("MaterialNumber")]
        public int MaterialNumber { get; set; }

        [IsQueryableAttribute()]
        [JsonPropertyName("MainProductGroup")]
        public string MainProductGroup { get; set; }

        [IsQueryableAttribute()]
        [JsonPropertyName("RetailProductGroup")]
        public string RetailProductGroup { get; set; }

        [IsQueryableAttribute()]
        [JsonPropertyName("Brand")]
        public string Brand { get; set; }

        [IsQueryableAttribute()]
        [JsonPropertyName("Gender")]
        public string Gender { get; set; }

        [IsQueryableAttribute()]
        [JsonPropertyName("Line")]
        public string Line { get; set; }

        [IsQueryableAttribute()]
        [JsonPropertyName("FormMaterialNumber")]
        public int FormMaterialNumber { get; set; }

        [IsQueryableAttribute()]
        [JsonPropertyName("QualityMaterialNumber")]
        public int QualityMaterialNumber { get; set; }

        [IsQueryableAttribute()]
        [JsonPropertyName("MaterialText")]
        public string MaterialText { get; set; }

    }
}
