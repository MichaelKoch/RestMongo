using System;
using Data.Interfaces;
using MongoDB.Bson;

namespace Data.Models
{
    public abstract class Document : IDocument
    {
        public ObjectId Id { get; set; }

        public DateTime CreatedAt => Id.CreationTime;
    }
}