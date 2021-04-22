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
            source = source.Where(s => s.EAN >= 0 && s.UPC >=0).ToList();
            foreach (var i in source)
            {
                i.Id = i.EAN.ToString();
                if(i.EAN.ToString().Length != 11)
                {
                    i.EAN = 0;
                    i.UPC = i.EAN;
                }
                i.Timestamp = timestamp;
            }
            return this.Distinct(source);
        }
    }
}