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
            SyncProductColorSize();
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
