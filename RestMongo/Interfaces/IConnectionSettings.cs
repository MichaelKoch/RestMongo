using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RestMongo.Interfaces
{
    public interface IConnectionSettings
    {
        string ConnectionString
        { get; set; }

        string DatabaseName
        { get; set; }
    }
}
