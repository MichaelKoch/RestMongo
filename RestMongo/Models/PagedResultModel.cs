using System.Collections.Generic;
using System.Text.Json.Serialization;

public class PagedResultModel<TEntity>
{
    [JsonPropertyName("Top")]
    public int Top { get; set; }

    [JsonPropertyName("Skip")]
    public int Skip { get; set; }

    [JsonPropertyName("Total")]
    public int Total { get; set; }

    [JsonPropertyName("Values")]
    public IList<TEntity> Values { get; set; }
}