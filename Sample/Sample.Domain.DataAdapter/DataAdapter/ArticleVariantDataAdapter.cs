using MongoBase.Repositories;
using Sample.Domain.DataAdapter.Abstractions;
using Sample.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Sample.Domain.DataAdapter
{
    public class ArticleVariantDataAdapter: JsonSourceDataAdapter<ArticleVariant,ArticleVariant>
    {
        
    

        public override IList<ArticleVariant> Transform(IList<ArticleVariant> source)
        {
            long timestamp = DateTime.UtcNow.Ticks;
            source = source.Where(s => s.EAN >= 0).ToList();
            foreach (var i in source)
            {
                i.ChangedAt = timestamp;
            }
            return source.ToList();
        }
    }
}