using RestMongo.Data.Abstractions.Repository.Mongo.Configuration;
using RestMongo.Data.Repository.Configuration;

namespace RestMongo.Test.Helper
{
    public class ConfigHelper
    {
        public static ConnectionSettings _config = new ConnectionSettings()
        {
            ConnectionString = "mongodb://admin:admin@vehicle:27017",
            DatabaseName = "test"
        };

        public static IConnectionSettings GetMongoConfig()
        {
            return _config;
        }

    }
}
