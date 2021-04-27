using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using MongoBase.Attributes;
using MongoBase.Interfaces;
using MongoBase.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoBase.Models
{
    public abstract class LocalizedFeedDocument :LocalizedDocument, ILocalizedFeedDocument
    {
    
        public long Timestamp { get; set; }
    }
}
