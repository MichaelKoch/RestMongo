
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

            ProductContext context = new ProductContext(db);
            List<Task> waitfor = new List<Task>();
            waitfor.Add(Task.Run(() => SyncArticleVariants(context.ArticleVariants)));
            // waitfor.Add(Task.Run(() => SyncMaterialText(context.MaterialTexts)));
            // waitfor.Add(Task.Run(() => SyncMaterialComposition(context.MaterialCompositions)));
            // waitfor.Add(Task.Run(() => SyncMaterialClassification(context.MaterialClassifications)));
            Task.WaitAll(waitfor.ToArray());

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

        private static async void SyncArticleVariants(MongoRepository<ArticleVariant> repo)
        {
            var materialRepo = new MongoRepository<CollectionMaterial>(repo.ConnectionSettings);
            var dataAdapterArticles = new ArticleVariantDataAdapter();
            var dataAdapterCollectionMaterials = new CollectionMaterialAdapter();
            IList<ArticleVariant> articles = dataAdapterArticles.Extract().ToList();
            articles = dataAdapterArticles.Transform(articles);
            dataAdapterArticles.Load(articles, repo);
            var materials = dataAdapterCollectionMaterials.Transform(articles);
            dataAdapterCollectionMaterials.Load(materials, materialRepo);
        }

        private static async void SyncMaterialClassification(MongoRepository<MaterialClassification> repo)
        {
            var dataAdapter = new MaterialClassificationDataAdapter();
            var data = dataAdapter.Transform(dataAdapter.Extract().ToList());
            dataAdapter.Load(data, repo);
        }

        private static async void SyncMaterialComposition(MongoRepository<MaterialComposition> repo)
        {
            var dataAdapter = new MaterialCompositionDataAdapter();
            var data = dataAdapter.Transform(dataAdapter.Extract().ToList());
            dataAdapter.Load(data, repo);
        }

        private static async void SyncMaterialText(MongoRepository<MaterialText> repo)
        {
            var dataAdapter = new MaterialTextDataAdapter();
            var data = dataAdapter.Transform(dataAdapter.Extract().ToList());
            dataAdapter.Load(data, repo);
        }
    }
}
