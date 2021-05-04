using MongoDB.Bson.Serialization.Attributes;
using RestMongo.Interfaces;

namespace RestMongo.Models
{
    [BsonIgnoreExtraElements]
    public class LocalizedFeedDocument : LocalizedDocument, ILocalizedFeedDocument
    {
        public long Timestamp { get; set; }
    }
}
