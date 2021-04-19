using Sample.Domain.DataAdapter.Repositories;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using MongoBase.Models;
using System.Collections.Generic;
using MongoBase.Repositories;
using Sample.Domain.Models;

namespace Sample.Domain.DataAdapter
{
    internal static class Program
    {
        private static readonly ConnectionSettings db = new ()
            {
                DatabaseName = "DomainProduct",
                ConnectionString = "mongodb://localhost"
            };
        internal static void Main(string[] args)
        {
            var source = new ProductColorSizeSourceRepository();
            var matnrs = source.AsQueryable().GroupBy(c => c.MaterialNumber).Select(m => m.First().MaterialNumber);
            File.WriteAllText(@"c:\temp\test50.json", JsonSerializer.Serialize(matnrs.Take(50)));
            File.WriteAllText(@"c:\temp\test250.json", JsonSerializer.Serialize(matnrs.Take(250)));
            File.WriteAllText(@"c:\temp\test500.json", JsonSerializer.Serialize(matnrs.Take(500)));
            File.WriteAllText(@"c:\temp\test1000.json", JsonSerializer.Serialize(matnrs.Take(1000)));
        }
        

        private static void createMatnrSampleLists()
        {
            var source = new ProductColorSizeSourceRepository();
            var matnrs = source.AsQueryable().Select(m => m.MaterialNumber);
            File.WriteAllText(@"c:\temp\test50.json", JsonSerializer.Serialize(matnrs.Take(50)));
            File.WriteAllText(@"c:\temp\test250.json", JsonSerializer.Serialize(matnrs.Take(250)));
            File.WriteAllText(@"c:\temp\test500.json", JsonSerializer.Serialize(matnrs.Take(500)));
            File.WriteAllText(@"c:\temp\test1000.json", JsonSerializer.Serialize(matnrs.Take(1000)));

        }

        private static void SyncProductColorSize()
        {
            var db = new ConnectionSettings()
            {
                DatabaseName = "DomainProduct",
                ConnectionString = "mongodb://192.168.2.210"
            };
            var source = new ProductColorSizeSourceRepository();
            var target = new Repository<ProductColorSize>(db);
            source.Sync(target);
        }
    }
}
