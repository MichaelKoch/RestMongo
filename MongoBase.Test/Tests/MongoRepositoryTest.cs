using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoBase;
using MongoBase.Exceptions;
using MongoBase.Interfaces;
using MongoBase.Models;
using MongoBase.Repositories;
using MongoBase.Test.Helper;
using MongoBase.Test.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.Json;

namespace MongoBase.Test
{
    [TestClass]
    public class MongoRepositoryTest
    {



        [TestMethod]
        public void Query()
        {
            var repo = DataHelper.getRepository<TestModel>();
            var context = Guid.NewGuid().ToString();
            var testData = DataHelper.CreateTestData(context, 200);
            repo.InsertManyAsync(testData).Wait();
            var query = new ExpandoObject();
            query.TryAdd("Context", context);

            var existing = repo.Query(JsonSerializer.Serialize(query), "", 200);
            Assert.IsTrue(existing.Values.Count() == 200);
            Assert.IsTrue(existing.Total == 200);
            Assert.IsTrue(existing.Top == 200);

            DataHelper.Cleanup(repo, context);

        }

        [TestMethod]
        public void QueryOrderBy()
        {
            var repo = DataHelper.getRepository<TestModel>();
            var context = Guid.NewGuid().ToString();
            var testData = DataHelper.CreateTestData(context, 200);
            repo.InsertManyAsync(testData).Wait();
            var query = new ExpandoObject();
            query.TryAdd("Context", context);

            var existing = repo.Query(JsonSerializer.Serialize(query), "Id desc,Name asc", 200);
            Assert.IsTrue(existing.Values.Count() == 200);
            Assert.IsTrue(existing.Total == 200);
            Assert.IsTrue(existing.Top == 200);

            DataHelper.Cleanup(repo, context);

        }


        [TestMethod]
        public void GetServerTimeStamp()
        {
            var repo = DataHelper.getRepository<TestModel>();
            var timestamp = repo.GetServerTimeStamp();
            Assert.IsTrue(timestamp > 0);

        }
        [TestMethod]
        public void StoreSyncData()
        {
            var repo = DataHelper.getRepository<TestModelFeed>();
            var context = Guid.NewGuid().ToString();
            var testData = DataHelper.CreateTestDataFeed(context, 200);
            repo.StoreSyncDelta(testData);
            var stored = repo.FilterBy(s => s.Context == context);
            Assert.IsTrue(stored.Count() == 200);

            context = Guid.NewGuid().ToString();
            testData = DataHelper.CreateTestDataFeed(context, 200);
            repo.StoreSyncDelta(testData);
            stored = repo.FilterBy(s => s.Context == context);
            Assert.IsTrue(stored.Count() == 200);
            DataHelper.Cleanup(repo, context);

        }

        [TestMethod]
        public void QueryMaxPageSize()
        {
            var maxPageSize = 100;
            var repo = DataHelper.getRepository<TestModel>();
            var context = Guid.NewGuid().ToString();
            var testData = DataHelper.CreateTestData(context, maxPageSize + 10);
            repo.InsertManyAsync(testData).Wait();
            var query = new ExpandoObject();
            query.TryAdd("Context", context);
            Assert.ThrowsException<PageSizeExeededException>(() =>
            {
               repo.Query(JsonSerializer.Serialize(query), "", maxPageSize);
            });

            DataHelper.Cleanup(repo, context);

        }



        [TestMethod]
        public void GetLastChanged()
        {
            var repo = DataHelper.getRepository<TestModelFeed>();
            var context = Guid.NewGuid().ToString();
            var testData = DataHelper.CreateTestDataFeed(context, 200);
            repo.InsertManyAsync(testData).Wait();
            var maxLastChanged = repo.AsQueryable().Max(c => c.Timestamp);
            Assert.IsTrue(maxLastChanged == repo.GetMaxLastChanged());

            DataHelper.Cleanup(repo, context);

        }
        [TestMethod]
        public void InsertOneIFeedDocument()
        {
            var repo = DataHelper.getRepository<TestModelFeed>();
            var context = Guid.NewGuid().ToString();
            repo.InsertOne(new TestModelFeed() { Context = context });
            var inserted = repo.AsQueryable().FirstOrDefault(c => c.Context == context);
            Assert.IsNotNull(inserted);
            DataHelper.Cleanup(repo, context);
        }

     


        [TestMethod]
        public void InsertOneIFeedDocumentAsync()
        {
            var repo = DataHelper.getRepository<TestModelFeed>();
            var context = Guid.NewGuid().ToString();
            repo.InsertOneAsync(new TestModelFeed() { Context = context }).Wait();
            var inserted = repo.AsQueryable().FirstOrDefault(c => c.Context == context);
            Assert.IsNotNull(inserted);
            DataHelper.Cleanup(repo, context);
        }
        [TestMethod]
        public void InsertOne()
        {
            var repo = DataHelper.getRepository<TestModel>();
            var context = Guid.NewGuid().ToString();
            repo.InsertOne(new TestModel() { Context = context });
            var inserted = repo.AsQueryable().FirstOrDefault(c => c.Context == context);
            Assert.IsNotNull(inserted);
            DataHelper.Cleanup(repo, context);
        }
        [TestMethod]
        public void InsertOneAsync()
        {
            var repo = DataHelper.getRepository<TestModel>();
            var context = Guid.NewGuid().ToString();
            repo.InsertOneAsync(new TestModel() { Context = context }).Wait(); ;
            var inserted = repo.AsQueryable().FirstOrDefault(c => c.Context == context);
            Assert.IsNotNull(inserted);
            DataHelper.Cleanup(repo, context);
        }

        [TestMethod]
        public void FindOne()
        {
            var repo = DataHelper.getRepository<TestModel>();
            var context = Guid.NewGuid().ToString();
            repo.InsertOne(new TestModel() { Context = context });
            var inserted = repo.AsQueryable().FirstOrDefault(c => c.Context == context);
            Assert.IsNotNull(inserted);
            var found = repo.FindOne(c => c.Id == inserted.Id);
            Assert.IsNotNull(found);
            DataHelper.Cleanup(repo, context);
        }
        [TestMethod]
        public void FindOneAsync()
        {
            var repo = DataHelper.getRepository<TestModel>();
            var context = Guid.NewGuid().ToString();
            repo.InsertOneAsync(new TestModel() { Context = context }).Wait(); ;
            var inserted = repo.AsQueryable().FirstOrDefault(c => c.Context == context);
            Assert.IsNotNull(inserted);
            var found = repo.FindOneAsync(c => c.Id == inserted.Id).Result;
            Assert.IsNotNull(found);
            DataHelper.Cleanup(repo, context);
        }

        [TestMethod]
        public void FindByID()
        {
            var repo = DataHelper.getRepository<TestModel>();
            var context = Guid.NewGuid().ToString();
            repo.InsertOne(new TestModel() { Context = context });
            var inserted = repo.AsQueryable().FirstOrDefault(c => c.Context == context);
            Assert.IsNotNull(inserted);
            var found = repo.FindById(inserted.Id);
            Assert.IsNotNull(found);
            DataHelper.Cleanup(repo, context);
        }
        [TestMethod]
        public void FindByIDAsync()
        {
            var repo = DataHelper.getRepository<TestModel>();
            var context = Guid.NewGuid().ToString();
            repo.InsertOneAsync(new TestModel() { Context = context }).Wait(); ;
            var inserted = repo.AsQueryable().FirstOrDefault(c => c.Context == context);
            Assert.IsNotNull(inserted);
            Assert.IsNotNull(inserted);
            var found = repo.FindByIdAsync(inserted.Id).Result;
            Assert.IsNotNull(found);
            DataHelper.Cleanup(repo, context);
        }

        [TestMethod]
        public void DeleteById()
        {
            var repo = DataHelper.getRepository<TestModel>();
            var context = Guid.NewGuid().ToString();
            repo.InsertOne(new TestModel() { Context = context });
            var inserted = repo.AsQueryable().FirstOrDefault(c => c.Context == context);
            Assert.IsNotNull(inserted);
            repo.DeleteById(inserted.Id);
            var deleted = repo.AsQueryable().FirstOrDefault(c => c.Context == context);
            Assert.IsNull(deleted);
            DataHelper.Cleanup(repo, context);
        }
        [TestMethod]
        public void DeleteByIdAsync()
        {
            var repo = DataHelper.getRepository<TestModel>();
            var context = Guid.NewGuid().ToString();
            repo.InsertOneAsync(new TestModel() { Context = context }).Wait();
            var inserted = repo.AsQueryable().FirstOrDefault(c => c.Context == context);
            Assert.IsNotNull(inserted);
            repo.DeleteByIdAsync(inserted.Id).Wait();
            var deleted = repo.AsQueryable().FirstOrDefault(c => c.Context == context);
            Assert.IsNull(deleted);
            DataHelper.Cleanup(repo, context);
        }


        [TestMethod]
        public void InsertManyAsync()
        {

            var repo = DataHelper.getRepository<TestModel>();
            var context = Guid.NewGuid().ToString();
            var testData = DataHelper.CreateTestData(context, 1000);
            repo.InsertManyAsync(testData).Wait();
            var inserted = repo.AsQueryable().Where(c => c.Context == context).ToList();
            Assert.IsTrue(inserted.Count == testData.Count);

            DataHelper.Cleanup(repo, context);
        }

        [TestMethod]
        public void FilterBy()
        {
            var repo = DataHelper.getRepository<TestModel>();
            var context = Guid.NewGuid().ToString();
            var testData = DataHelper.CreateTestData(context, 1000);
            repo.InsertManyAsync(testData).Wait();
            var inserted = repo.FilterBy(c => c.Context == context).ToList();
            Assert.IsTrue(inserted.Count == testData.Count);
            DataHelper.Cleanup(repo, context);
        }

        [TestMethod]
        public void InsertMany()
        {

            var repo = DataHelper.getRepository<TestModel>();
            var context = Guid.NewGuid().ToString();
            var testData = DataHelper.CreateTestData(context, 1000);
            repo.InsertMany(testData);
            var inserted = repo.AsQueryable().Where(c => c.Context == context).ToList();
            Assert.IsTrue(inserted.Count == testData.Count);
            DataHelper.Cleanup(repo, context);
        }
        [TestMethod]
        public void InsertManyFeedDocuments()
        {

            var repo = DataHelper.getRepository<TestModelFeed>();
            var context = Guid.NewGuid().ToString();
            var testData = DataHelper.CreateTestDataFeed(context, 1000);
            repo.InsertMany(testData);
            var inserted = repo.AsQueryable().Where(c => c.Context == context).ToList();
            Assert.IsTrue(inserted.Count == testData.Count);
            DataHelper.Cleanup(repo, context);
        }
        [TestMethod]
        public void DeleteOneAsync()
        {
            var repo = DataHelper.getRepository<TestModel>();
            var context = Guid.NewGuid().ToString();
            var testData = DataHelper.CreateTestData(context, 1000);

            repo.InsertMany(testData);
            var firstOne = repo.AsQueryable().Take(1).ToList()[0];
            repo.DeleteByIdAsync(firstOne.Id).Wait();
            firstOne = repo.AsQueryable().SingleOrDefault(c => c.Id == firstOne.Id);
            Assert.IsNull(firstOne);

            DataHelper.Cleanup(repo, context);
        }

        [TestMethod]
        public void DeleteOne()
        {
            var repo = DataHelper.getRepository<TestModel>();
            var context = Guid.NewGuid().ToString();
            var testData = DataHelper.CreateTestData(context, 1000);

            repo.InsertMany(testData);
            var firstOne = repo.AsQueryable().Take(1).ToList()[0];
            repo.DeleteById(firstOne.Id);
            firstOne = repo.AsQueryable().SingleOrDefault(c => c.Id == firstOne.Id);
            Assert.IsNull(firstOne);
            DataHelper.Cleanup(repo, context);
        }

        [TestMethod]
        public void UpdateOne()
        {
            var repo = DataHelper.getRepository<TestModel>();
            var context = Guid.NewGuid().ToString();
            var testData = DataHelper.CreateTestData(context, 1);
            repo.InsertMany(testData);
            var firstOne = repo.AsQueryable().Take(1).ToList()[0];
            firstOne.Name = "UPDATED";
            repo.ReplaceOne(firstOne);
            var updated = repo.AsQueryable().SingleOrDefault(c => c.Id == firstOne.Id);
            Assert.IsTrue(updated != null);
            Assert.IsTrue(updated.Name == "UPDATED");
            DataHelper.Cleanup(repo, context);
        }
        [TestMethod]
        public void UpdateOneAsync()
        {
            var repo = DataHelper.getRepository<TestModelFeed>();
            var context = Guid.NewGuid().ToString();
            var testData = DataHelper.CreateTestDataFeed(context, 1);
            repo.InsertManyAsync(testData).Wait();
            var firstOne = repo.AsQueryable().Take(1).ToList()[0];
            firstOne.Name = "UPDATED";
            repo.ReplaceOneAsync(firstOne).Wait();
            var updated = repo.AsQueryable().SingleOrDefault(c => c.Id == firstOne.Id);
            Assert.IsTrue(updated != null);
            Assert.IsTrue(updated.Name == "UPDATED");
            Assert.IsTrue(updated.Timestamp > 0);


            DataHelper.Cleanup(repo, context);
        }



        [TestMethod]
        public void Queryable()
        {
            var repo = DataHelper.getRepository<TestModel>();
            var context = Guid.NewGuid().ToString();
            var testData = DataHelper.CreateTestData(context, 100);
            repo.InsertMany(testData);
            var query = repo.AsQueryable().Where(c => c.Name.StartsWith("NAME"));
            query = query.Where(c => c.Context == context);
            var list = query.ToList();
            Assert.IsTrue(list.Count() == 100);
            DataHelper.Cleanup(repo, context);
        }

        [TestMethod]
        public void MongoQuery()
        {
            var repo = DataHelper.getRepository<TestModel>();
            var context = Guid.NewGuid().ToString();
            var testData = DataHelper.CreateTestData(context, 100);
            repo.InsertMany(testData);
            var query = new ExpandoObject();
            query.TryAdd("Context", context);
            var result = repo.Query(System.Text.Json.JsonSerializer.Serialize(query), "");
            Assert.IsTrue(result.Values.Count() == 100);
            Assert.IsTrue(result.Total == 100);
            DataHelper.Cleanup(repo, context);
        }
    }
}
