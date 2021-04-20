using MongoBase.Attributes;
using System;
using System.Text.Json.Serialization;

namespace Sample.Domain.Models
{
    [MongoBase.Attributes.BsonCollection("ArticleVariant")]
    public class ArticleVariant: MongoBase.Models.BaseDocument
    {
        public override string Id
        {
            get {
                    if(!string.IsNullOrEmpty(EAN))
                    {
                        return this.EAN;
                    }else if(!string.IsNullOrEmpty(UPC))
                    {
                        return this.UPC;
                    }
                    else
                    {
                        return Guid.NewGuid().ToString();
                    }
                }
        }
        [IsQueryableAttribute()]
        [JsonPropertyName("MaterialNumber")]
        public string MaterialNumber { get; set; }

        [IsQueryableAttribute()]
        [JsonPropertyName("ColorSize")]
        public string ColorSize { get; set; }

        [IsQueryableAttribute()]
        [JsonPropertyName("EAN11")]
        public string EAN { get; set; }

        [IsQueryableAttribute()]
        [JsonPropertyName("UPC")]
        public string UPC { get; set; }

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
        public string FormMaterialNumber { get; set; }

        [IsQueryableAttribute()]
        [JsonPropertyName("QualityMaterialNumber")]
        public string QualityMaterialNumber { get; set; }

        [IsQueryableAttribute()]
        [JsonPropertyName("MaterialText")]
        public string MaterialText { get; set; }

        [IsQueryableAttribute()]
        [JsonPropertyName("Color")]
        public string Color { get; set; }

        [IsQueryableAttribute()]
        [JsonPropertyName("Size1")]
        public string Size1 { get; set; }

        [IsQueryableAttribute()]
        [JsonPropertyName("Size2")]
        public string Size2 { get; set; }
    }
}
