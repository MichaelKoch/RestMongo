using MongoBase.Attributes;
using System;
using System.Text.Json.Serialization;

namespace Sample.Domain.Models
{
    [MongoBase.Attributes.BsonCollection("ProductColorSize")]
    public class ProductColorSize: MongoBase.Models.BaseDocument
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
        [SchemaAttribute(isSimple: true)]
        [JsonPropertyName("MaterialNumber")]
        public string MaterialNumber { get; set; }

        [SchemaAttribute(isSimple: true)]
        [JsonPropertyName("ColorSize")]
        public string ColorSize { get; set; }

        [SchemaAttribute(isSimple: true)]
        [JsonPropertyName("EAN11")]
        public string EAN { get; set; }

        [SchemaAttribute(isSimple: true)]
        [JsonPropertyName("UPC")]
        public string UPC { get; set; }

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

        [SchemaAttribute(isSimple: true)]
        [JsonPropertyName("Color")]
        public string Color { get; set; }

        [SchemaAttribute(isSimple: true)]
        [JsonPropertyName("Size1")]
        public string Size1 { get; set; }

        [SchemaAttribute(isSimple: true)]
        [JsonPropertyName("Size2")]
        public string Size2 { get; set; }
    }
}
