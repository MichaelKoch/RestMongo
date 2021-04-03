using System;
using Data.Interfaces;
using MongoDB.Bson;

namespace Data.Models
{
    public class MongoDbSettings : IMongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}