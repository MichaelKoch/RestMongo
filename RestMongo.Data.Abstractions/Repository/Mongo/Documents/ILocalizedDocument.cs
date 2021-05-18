namespace RestMongo.Data.Abstractions.Repository.Mongo.Documents
{
    public interface ILocalizedDocument : IDocument
    {
        public string Locale { get; set; }
    }
}
