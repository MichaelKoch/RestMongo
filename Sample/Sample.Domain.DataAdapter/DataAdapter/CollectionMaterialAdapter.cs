using MongoBase.Repositories;
using Sample.Domain.DataAdapter.Abstractions;
using Sample.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Sample.Domain.DataAdapter
{
    public class CollectionMaterialAdapter : JsonSourceDataAdapter<ArticleVariant,CollectionMaterial>
    {
        
    

        public override IList<CollectionMaterial> Transform(IList<ArticleVariant> source)
        {
            long timestamp = DateTime.UtcNow.Ticks;
            var target = JsonSerializer.Deserialize<IList<CollectionMaterial>>(JsonSerializer.Serialize(source));
            foreach(var t in target)
            {
                t.Id = t.MaterialNumber.ToString();
                t.Timestamp = timestamp;
            }
            return this.Distinct(target);
        }
    }
}