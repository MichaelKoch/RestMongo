
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using MongoBase.Models;
using System.Collections.Generic;
using MongoBase.Repositories;
using Sample.Domain.Models;
using System.Threading.Tasks;

namespace Sample.Domain.DataAdapter
{
    internal static class Program
    {
        //TODO : GET FROM CONFIG
        private static readonly ConnectionSettings db = new()
        {
            DatabaseName = "DomainProduct",
            ConnectionString = "mongodb://admin:admin@localhost"
        };
        internal static void Main(string[] args)
        {

            //SyncArticleVariants();
            //SyncMaterialText();
            //SyncMaterialComposition();
            SyncMaterialClassification();

        }


        private static void createMatnrSampleLists()
        {
            // var source = new ProductColorSizeSourceRepository();
            // var matnrs = source.AsQueryable().Select(m => m.MaterialNumber);
            // File.WriteAllText(@"c:\temp\test50.json", JsonSerializer.Serialize(matnrs.Take(50)));
            // File.WriteAllText(@"c:\temp\test250.json", JsonSerializer.Serialize(matnrs.Take(250)));
            // File.WriteAllText(@"c:\temp\test500.json", JsonSerializer.Serialize(matnrs.Take(500)));
            // File.WriteAllText(@"c:\temp\test1000.json", JsonSerializer.Serialize(matnrs.Take(1000)));

        }

        private static void SyncArticleVariants()
        {
            var dataAdapter = new ArticleVariantDataAdapter();
            var target = new Repository<ArticleVariant>(db);
            var data = dataAdapter.Transform(dataAdapter.Extract().ToList());
            dataAdapter.Load(data, target);
        }

        private static void SyncMaterialClassification()
        {
            var dataAdapter = new MaterialClassificationDataAdapter();
            var target = new Repository<MaterialClassification>(db);
            var data = dataAdapter.Transform(dataAdapter.Extract().ToList());
            dataAdapter.Load(data, target);
        }
        private static void SyncMaterialComposition()
        {
            var dataAdapter = new MaterialCompositionDataAdapter();
            var target = new Repository<MaterialComposition>(db);
            var data = dataAdapter.Transform(dataAdapter.Extract().ToList());
            dataAdapter.Load(data, target);
        }
        private static void SyncMaterialText()
        {
            var dataAdapter = new MaterialTextDataAdapter();
            var target = new Repository<MaterialText>(db);
            var data = dataAdapter.Transform(dataAdapter.Extract().ToList());
            dataAdapter.Load(data, target);
        }
    }
}
