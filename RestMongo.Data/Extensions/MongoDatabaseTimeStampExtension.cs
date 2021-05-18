using MongoDB.Bson;
using MongoDB.Driver;

namespace RestMongo.Data.Extensions
{
    static class MongoDatabaseTimeStampExtension
    {
        public static long GetServerTimeStap(this IMongoDatabase Instance)
        {

            var serverStatusCmd = new BsonDocumentCommand<BsonDocument>(new BsonDocument { { "serverStatus", 1 } });
            return Instance.RunCommand(serverStatusCmd)["localTime"].ToUniversalTime().Ticks;
        }
    }
}
