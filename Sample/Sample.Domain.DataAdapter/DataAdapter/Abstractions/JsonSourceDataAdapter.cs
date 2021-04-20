using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Sample.Domain.Models;

namespace Sample.Domain.DataAdapter.Abstractions
{

   public abstract class JsonSourceDataAdapter<TSource,TTarget>
   {
        public JsonSourceDataAdapter()
        {
            
        }
        public void Load(IEnumerable<TSource> source ,Repository<TTarget> repo)
        {
            
            if (repo is null)
            {
                throw new System.ArgumentNullException(nameof(repo));
            }
            long lastKnowTimestamp = repo.GetMaxLastChanged();
            List<ProductColorSize> delta = source.Where(d=>
                d.ChangedAt > lastKnowTimestamp &&
                d.EAN != null
            ).ToList();
            repo.StoreSyncDelta(delta);
        }

        protected virtual IEnumerable<TSource> Extract()
        {
            string dataFileName = $"data/{typeof(TSource).Name}.sample.json";
             if(!File.Exists(dataFileName))
            {
                throw new FileNotFoundException("data sample file not found");
            }
            return JsonSerializer.Deserialize<List<TSource>>(File.ReadAllText(dataFileName)); 
        }

        protected abstract IList<TTarget> Transform(IList<TSource> source)
        {
            return source;
        }
   }
}
