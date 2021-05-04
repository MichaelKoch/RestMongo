using MongoDB.Bson.Serialization.Attributes;
using RestMongo.Interfaces;

namespace RestMongo.Models
{
    [BsonIgnoreExtraElements]
    public class LocalizedDocument : BaseDocument, ILocalizedDocument
    {
        public virtual string Locale { get; set; }
    }
}