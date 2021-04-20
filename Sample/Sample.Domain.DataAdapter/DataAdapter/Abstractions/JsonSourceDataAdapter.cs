using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MongoBase.Interfaces;
using MongoBase.Repositories;
using Sample.Domain.Models;

namespace Sample.Domain.DataAdapter.Abstractions
{

   public abstract class JsonSourceDataAdapter<TSource,TTarget>
          where TSource : IDocument 
          where TTarget : IDocument
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
            IList<TTarget> converted = Transform(source.ToList());
            IList<TTarget> delta = converted.Where(d=>
                d.ChangedAt > lastKnowTimestamp
            ).ToList();
            repo.StoreSyncDelta(delta);
        }

        public virtual IEnumerable<TSource> Extract()
        {
            string dataFileName = $"data/{typeof(TSource).Name}.sample.json";
             if(!File.Exists(dataFileName))
            {
                throw new FileNotFoundException("data sample file not found");
            }
            return JsonSerializer.Deserialize<List<TSource>>(File.ReadAllText(dataFileName)); 
        }

        public abstract IList<TTarget> Transform(IList<TSource> source);
      
   }
}
