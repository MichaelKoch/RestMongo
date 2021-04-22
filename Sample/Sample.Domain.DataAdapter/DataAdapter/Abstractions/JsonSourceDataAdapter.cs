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
          where TSource : IFeedDocument 
          where TTarget : IFeedDocument
    {    
        public JsonSourceDataAdapter()
        {
            
        }

        protected IList<TTarget> Distinct(IEnumerable<TTarget> list)
        {
            var known = new HashSet<string>();
            var distinct = list.Where(element => known.Add(element.Id)).ToList();
            return distinct;
        }
        public void Load(IEnumerable<TTarget> data , MongoRepository<TTarget> repo)
        {
            
            if (repo is null)
            {
                throw new System.ArgumentNullException(nameof(repo));
            }
            long lastKnowTimestamp = repo.GetMaxLastChanged();
            IList<TTarget> delta = data.Where(d=>
                d.Timestamp > lastKnowTimestamp
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
