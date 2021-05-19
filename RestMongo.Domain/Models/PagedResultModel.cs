using System.Collections.Generic;
using System.Text.Json.Serialization;
using RestMongo.Domain.Abstractions.Models;

namespace RestMongo.Domain.Models
{
    public class PagedResultModel<TEntity> : IPagedResultModel<TEntity>
    {
        [JsonPropertyName("Top")] public int Top { get; set; }

        [JsonPropertyName("Skip")] public int Skip { get; set; }

        [JsonPropertyName("Total")] public int Total { get; set; }

        [JsonPropertyName("Values")] public IList<TEntity> Values { get; set; }
    }
}