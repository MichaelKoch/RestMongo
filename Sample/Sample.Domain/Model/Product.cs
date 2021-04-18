using MongoBase.Attributes;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sample.Domain.Models
{
    [MongoBase.Attributes.BsonCollection("viewProduct")]
    public class Product: MongoBase.Models.BaseDocument
    {
        [SchemaAttribute(isSimple: true)]
        [JsonPropertyName("MaterialNumber")]
        public string MaterialNumber { get; set; }

        [SchemaAttribute(isSimple: true)]
        [JsonPropertyName("MainProductGroup")]
        public string MainProductGroup { get; set; }

        [SchemaAttribute(isSimple: true)]
        [JsonPropertyName("RetailProductGroup")]
        public string RetailProductGroup { get; set; }

        [SchemaAttribute(isSimple: true)]
        [JsonPropertyName("Brand")]
        public string Brand { get; set; }

        [SchemaAttribute(isSimple: true)]
        [JsonPropertyName("Gender")]
        public string Gender { get; set; }

        [SchemaAttribute(isSimple: true)]
        [JsonPropertyName("Line")]
        public string Line { get; set; }

        [SchemaAttribute(isSimple: true)]
        [JsonPropertyName("FormMaterialNumber")]
        public string FormMaterialNumber { get; set; }

        [SchemaAttribute(isSimple: true)]
        [JsonPropertyName("QualityMaterialNumber")]
        public string QualityMaterialNumber { get; set; }

        [SchemaAttribute(isSimple: true)]
        [JsonPropertyName("MaterialText")]
        public string MaterialText { get; set; }


        public List<ProductColorSize> Variants { get; set; }

        public List<ProductColor> Colors { get; set; }


    }
}
