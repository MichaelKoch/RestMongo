using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using MongoBase.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoBase.Interfaces
{
    public interface IFeedDocument:IDocument
    {
        [IsQueryableAttribute()]
        [JsonPropertyName("Timestamp")]
        public long Timestamp { get; set; }

   
    }
}
