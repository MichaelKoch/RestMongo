using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Mon
{
    public interface IConnectionSettings
    {
        string ConnectionString
        { get; set; }

        string DatabaseName
        { get; set; }
    }
}
