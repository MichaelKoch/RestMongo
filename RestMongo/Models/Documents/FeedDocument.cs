using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using RestMongo.Attributes;
using RestMongo.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RestMongo.Models
{
    [BsonIgnoreExtraElements]
    public class FeedDocument : BaseDocument, IFeedDocument
    {
        [IsQueryableAttribute()]
        public long Timestamp { get; set; }
    }
}