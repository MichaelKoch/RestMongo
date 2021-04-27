using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using MongoBase.Attributes;
using MongoBase.Interfaces;
using MongoDB.Bson;

namespace MongoBase.Models
{
    public abstract class LocalizedDocument : BaseDocument, ILocalizedDocument
    {
        public string Locale { get; set; }
    }
}