using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RestMongo.Data.Abstractions.Repository.Mongo.Documents
{
    public interface IDocument: IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
