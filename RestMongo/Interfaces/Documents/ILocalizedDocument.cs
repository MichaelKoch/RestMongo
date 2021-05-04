namespace RestMongo.Interfaces
{
    public interface ILocalizedDocument : IDocument
    {
        public string Locale { get; set; }
    }
}
