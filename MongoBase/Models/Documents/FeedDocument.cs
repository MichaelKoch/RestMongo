using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using MongoBase.Attributes;
using MongoBase.Interfaces;
using MongoDB.Bson;

namespace MongoBase.Models
{
    public abstract class FeedDocument : BaseDocument, IFeedDocument
    {
        public long Timestamp { get; set; }
    }
}