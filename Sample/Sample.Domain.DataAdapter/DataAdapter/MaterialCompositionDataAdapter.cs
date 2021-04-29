using RestMongo.Interfaces;
using RestMongo.Repositories;
using Sample.Domain.DataAdapter.Abstractions;
using Sample.Domain.Models.Enities;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Sample.Domain.DataAdapter
{
    public class MaterialCompositionDataAdapter : JsonSourceDataAdapter<MaterialComposition, MaterialComposition>
    {



        public override IList<MaterialComposition> Transform(IList<MaterialComposition> source)
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