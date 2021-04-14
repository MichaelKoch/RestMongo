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
            //SyncProductColorSize();
            Sample.Domain.Initializer.Run(db);
            //    var target = new Repository<Product>(db);
            //    var t = target.AsQueryable().Where(
            //        i=>
            //             i.Brand == "01" &&
            //             i.MainProductGroup == "001"
            //    ).ToList();
        }

        private static void SyncProductColorSize()
        {
            var db = new ConnectionSettings()
            {
                DatabaseName = "DomainProduct",
                ConnectionString = "mongodb://localhost"
            };
            var source = new ProductColorSizeSourceRepository();
            var target = new Repository<ProductColorSize>(db);
            source.Sync(target);
        }
    }
}
