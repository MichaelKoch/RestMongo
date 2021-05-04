using RestMongo.Interfaces;

namespace RestMongo.Models
{
    public class ConnectionSettings : IConnectionSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}