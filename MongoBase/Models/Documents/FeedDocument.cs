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
    public  class FeedDocument : BaseDocument, IFeedDocument
    {
        [IsQueryableAttribute()]
        public long Timestamp { get; set; }
    }
}