using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using MongoBase.Attributes;
using MongoBase.Interfaces;
using MongoDB.Bson;

namespace MongoBase.Models
{
    public class BaseDocument : IDocument
    {
        [SchemaAttribute(isSimple: true)]
        [JsonPropertyName("Id")]
        public virtual string Id { get; set; }


        [SchemaAttribute(isSimple: true)]
        [JsonPropertyName("ChangedAt")]
        public virtual long ChangedAt { get; set; }
        public BaseDocument()
        {
            this.Id = Guid.NewGuid().ToString();
        }

    }
}