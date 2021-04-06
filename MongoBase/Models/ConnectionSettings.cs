using System;
using MongoBase.Interfaces;
using MongoDB.Bson;

namespace MongoBase.Models
{
    public class ConnectionSettings : IConnectionSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}