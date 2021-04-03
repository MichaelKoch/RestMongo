using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Data.Interfaces
{
    public interface IMongoDbSettings
    {
        string ConnectionString
        { get; set; }

         string DatabaseName
        { get; set; }
    }
}
