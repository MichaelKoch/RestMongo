using System.Collections.Generic;
using System.Text.Json.Serialization;
using MongoBase.Interfaces;

public class PagedResultModel<TDocument> where TDocument : IDocument
{
    [JsonPropertyName("Top")]
    public int Top { get; set; }

    [JsonPropertyName("Skip")]
    public int Skip { get; set; }

    [JsonPropertyName("Total")]
    public int Total { get; set; }

    [JsonPropertyName("Values")]
    public List<TDocument> Values { get; set; }
}