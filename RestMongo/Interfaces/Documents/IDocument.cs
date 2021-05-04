using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RestMongo.Interfaces
{
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

    }
}
