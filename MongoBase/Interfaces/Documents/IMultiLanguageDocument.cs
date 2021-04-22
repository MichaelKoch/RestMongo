using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoBase.Interfaces
{
    public interface IMultiLanguageDocument
    {
        public string Locale { get; set; }
    }
}
