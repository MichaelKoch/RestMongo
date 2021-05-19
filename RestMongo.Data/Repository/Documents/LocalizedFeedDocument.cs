using MongoDB.Bson.Serialization.Attributes;
using RestMongo.Data.Abstractions.Repository.Mongo.Documents;

namespace RestMongo.Data.Repository.Documents
{
    [BsonIgnoreExtraElements]
    public class LocalizedFeedDocument : LocalizedDocument, ILocalizedFeedDocument
    {
        public long Timestamp { get; set; }
    }
}
