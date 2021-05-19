using System.Collections.Generic;

namespace RestMongo.Domain.Abstractions.Models
{
    public interface IPagedResultModel<TEntity>
    {
        int Top { get; set; }
        int Skip { get; set; }
        int Total { get; set; }
        IList<TEntity> Values { get; set; }
    }
}