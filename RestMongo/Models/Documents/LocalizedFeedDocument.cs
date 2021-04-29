using RestMongo.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace RestMongo.Models
{
    [BsonIgnoreExtraElements]
    public class LocalizedFeedDocument : LocalizedDocument, ILocalizedFeedDocument
    {
        public long Timestamp { get; set; }
    }
}
