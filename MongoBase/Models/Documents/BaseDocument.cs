using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using MongoBase.Attributes;
using MongoBase.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoBase.Models
{
    [BsonIgnoreExtraElements]
    public abstract class BaseDocument : IDocument
    {
        [IsQueryableAttribute]
        [JsonPropertyName("Id")]
        public virtual string Id { get; set; }


        public BaseDocument()
        {
            this.Id = Guid.NewGuid().ToString();
        }

    }
}