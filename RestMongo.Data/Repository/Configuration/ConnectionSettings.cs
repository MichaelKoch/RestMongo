using RestMongo.Data.Abstractions.Repository.Mongo.Configuration;

namespace RestMongo.Data.Repository.Configuration
{
    public class ConnectionSettings : IConnectionSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}