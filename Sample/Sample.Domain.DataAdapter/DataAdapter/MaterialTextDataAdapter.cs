using MongoBase.Repositories;
using Sample.Domain.DataAdapter.Abstractions;
using Sample.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Sample.Domain.DataAdapter
{
    public class MaterialTextDataAdapter : JsonSourceDataAdapter<MaterialText, MaterialText>
    {
        
    

        public override IList<MaterialText> Transform(IList<MaterialText> source)
        {

            long timestamp = DateTime.UtcNow.Ticks;
            
            foreach (var i in source)
            {
                i.ChangedAt = timestamp;
            }
            return source.ToList();
        }
    }
}