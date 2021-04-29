using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RestMongo.Interfaces
{
    public interface ILocalizedDocument : IDocument
    {
        public string Locale { get; set; }
    }
}
