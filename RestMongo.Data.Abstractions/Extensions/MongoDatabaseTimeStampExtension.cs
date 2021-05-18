using MongoDB.Bson;
using MongoDB.Driver;

namespace RestMongo.Data.Abstractions.Extensions
{
    public static class MongoDatabaseTimeStampExtension
    {
        public static long GetServerTimeStamp(this IMongoDatabase Instance)
        {

            var serverStatusCmd = new BsonDocumentCommand<BsonDocument>(new BsonDocument { { "serverStatus", 1 } });
            return Instance.RunCommand(serverStatusCmd)["localTime"].ToUniversalTime().Ticks;
        }
    }
}
