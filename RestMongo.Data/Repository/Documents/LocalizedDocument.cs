using MongoDB.Bson.Serialization.Attributes;
using RestMongo.Data.Abstractions.Repository.Mongo.Documents;

namespace RestMongo.Data.Repository.Documents
{
    [BsonIgnoreExtraElements]
    public class LocalizedDocument : BaseDocument, ILocalizedDocument
    {
        public virtual string Locale { get; set; }
    }
}