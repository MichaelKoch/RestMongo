using MongoBase.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoBase.Models
{
    [BsonIgnoreExtraElements]
    public  class LocalizedFeedDocument :LocalizedDocument, ILocalizedFeedDocument
    {
    
        public long Timestamp { get; set; }
    }
}
