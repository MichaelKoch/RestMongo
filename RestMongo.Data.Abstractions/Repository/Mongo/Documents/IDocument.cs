using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RestMongo.Data.Abstractions.Repository.Mongo.Documents
{
    public interface IDocument: IEntity
    {
        [BsonId]
        public string Id { get; set; }
    }
}
