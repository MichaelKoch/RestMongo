namespace RestMongo.Data.Abstractions.Repository.Mongo.Configuration
{
    public interface IConnectionSettings
    {
        string ConnectionString { get; set; }

        string DatabaseName { get; set; }
    }
}