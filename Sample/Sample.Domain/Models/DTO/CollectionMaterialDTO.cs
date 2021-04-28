using MongoBase.Attributes;
using MongoBase.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sample.Domain.Models.Enities
{
    
    public class CollectionMaterialDTO
    {
        [JsonPropertyName("Locale")]
        public string Locale { get; set; }

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

        [IsQueryableAttribute()]
        [JsonPropertyName("Timestamp")]
        public long Timestamp { get; set; }

        [JsonPropertyName("SalesText")]
        public MaterialTextDTO SalesText { get; set; }

        [JsonPropertyName("Attributes")]
        public IList<MaterialClassificationDTO> Attributes { get; set; }

        [JsonPropertyName("Compositions")]
        public IList<MaterialCompositionDTO> Compositions { get; set; }

        [JsonPropertyName("Variants")]
        public IList<ArticleVariantDTO> Variants { get; set; }
    }
}
