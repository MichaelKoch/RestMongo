namespace RestMongo.Interfaces
{
    public interface IConnectionSettings
    {
        string ConnectionString
        { get; set; }

        string DatabaseName
        { get; set; }
    }
}
