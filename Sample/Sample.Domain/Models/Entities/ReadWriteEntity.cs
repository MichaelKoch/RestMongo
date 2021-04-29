using RestMongo.Attributes;
using RestMongo.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sample.Domain.Models.Enities
{
    [RestMongo.Attributes.BsonCollection("ReadWriteEntity")]
    [BsonIgnoreExtraElements]
    public class ReadWriteEntity : RestMongo.Models.FeedDocument, IFeedDocument
    {



        [IsQueryableAttribute()]
        [JsonPropertyName("Context")]
        public string Context { get; set; }


        [IsQueryableAttribute()]
        [JsonPropertyName("MaterialNumber")]
        public string MaterialNumber { get; set; }



    }
}
