using MongoBase.Repositories;
using Sample.Domain.DataAdapter.Abstractions;
using Sample.Domain.Models.Enities;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Sample.Domain.DataAdapter
{
    public class MaterialClassificationDataAdapter : JsonSourceDataAdapter<MaterialClassification, MaterialClassification>
    {
        
    

        public override IList<MaterialClassification> Transform(IList<MaterialClassification> source)
        {

            long timestamp = DateTime.UtcNow.Ticks;
            
            foreach (var i in source)
            {
                i.Timestamp = timestamp;
            }
            return this.Distinct(source);
        }
    }
}