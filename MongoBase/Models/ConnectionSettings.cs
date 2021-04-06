using System;
using Data.Interfaces;
using MongoDB.Bson;

namespace MongoBase
{
    public class ConnectionSettings : IMongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}