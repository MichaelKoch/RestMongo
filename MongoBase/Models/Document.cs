using System;
using MongoBase.Interfaces;
using MongoDB.Bson;

namespace MongoBase
{
    public abstract class BaseDocument : IDocument
    {
        public ObjectId Id { get; set; }
        public DateTime CreatedAt => Id.CreationTime;
        public long ChangedAt{get;set;}

    }
}