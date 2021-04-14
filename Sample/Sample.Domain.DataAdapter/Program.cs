using Sample.Domain.DataAdapter.Repositories;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using MongoBase.Models;
using System.Collections.Generic;

namespace Sample.Domain.DataAdapter
{
    class Program
    {
        static void Main(string[] args)
        {

            ProductSourceRepository repo = new ProductSourceRepository();
            List<Product> deltaMaterialized = new List<Product>();
            ProductRepository target = new ProductRepository(new() { ConnectionString = "mongodb://localhost", DatabaseName = "sample" });

            var delta = repo.getDeltaSince(target.getMaxLastChanged());
            foreach (var d in delta)
            {
                try
                {
                    var p = JsonSerializer.Deserialize<Product>(d.JSON);
                    p.Locale = d.Locale;
                    p.ChangedAt = d.CreatedDate.Ticks;
                    deltaMaterialized.Add(p);
                }
                catch (Exception ex)
                {

                }
            }
            target.StoreSyncDelta(deltaMaterialized);

            
            // List<Product> delta = source.AsQueryable().Where(c => c.ChangedAt > lastKnowSync).ToList();
            // target.StoreSyncDelta(delta);

        }
    }
}
