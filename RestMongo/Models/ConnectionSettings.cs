using System;
using RestMongo.Interfaces;
using MongoDB.Bson;

namespace RestMongo.Models
{
    public class ConnectionSettings : IConnectionSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}