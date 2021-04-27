using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoBase;
using MongoBase.Interfaces;
using MongoBase.Models;
using MongoBase.Repositories;
using MongoBase.Test.Helper;
using MongoBase.Test.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace MongoBase.Test
{
    [TestClass]
    public class RepositoryBaseCRUDTest
    {

    


        [TestMethod]
        public void InsertOne()
        {
            var repo = DataHelper.getRepository();
            var context = Guid.NewGuid().ToString();
            repo.InsertOne(new TestModel() { Context = context });
            var inserted = repo.AsQueryable().FirstOrDefault(c => c.Context == context);
            if(inserted == null)
            {
                throw new Exception("insert failed . can't retrieve instance by id");
            }
            DataHelper.Cleanup(repo, context);
        }
        [TestMethod]
        public void InsertOneAsync()
        {
            var repo = DataHelper.getRepository();
            var context = Guid.NewGuid().ToString();
            repo.InsertOneAsync(new TestModel() { Context = context }).Wait(); ;
            var inserted = repo.AsQueryable().FirstOrDefault(c => c.Context == context);
            if (inserted == null)
            {
                throw new Exception("insert failed . can't retrieve instance by id");
            }
            DataHelper.Cleanup(repo, context);
        }

        [TestMethod]
        public void InsertManyAsync()
        {

            var repo = DataHelper.getRepository();
            var context = Guid.NewGuid().ToString();
            var testData = DataHelper.CreateTestData(context, 100000);
            repo.InsertManyAsync(testData).Wait();
            var inserted = repo.AsQueryable().Where(c => c.Context == context).ToList();

            if (inserted.Count != testData.Count)
            {
                throw new System.Exception("Inserted items not matched");
            }

            DataHelper.Cleanup(repo, context);
        }

        [TestMethod]
        public void InsertMany()
        {

            var repo = DataHelper.getRepository();
            var context = Guid.NewGuid().ToString();
            var testData = DataHelper.CreateTestData(context, 100000);
            repo.InsertMany(testData);
            var inserted = repo.AsQueryable().Where(c => c.Context == context).ToList();

            if (inserted.Count != testData.Count)
            {
                throw new System.Exception("Inserted items not matched");
            }

            DataHelper.Cleanup(repo, context);
        }
        [TestMethod]
        public void DeleteOneAsync()
        {
            var repo = DataHelper.getRepository();
            var context = Guid.NewGuid().ToString();
            var testData = DataHelper.CreateTestData(context, 100000);

            repo.InsertMany(testData);
            var firstOne = repo.AsQueryable().Take(1).ToList()[0];
            repo.DeleteByIdAsync(firstOne.Id).Wait();
            firstOne = repo.AsQueryable().SingleOrDefault(c => c.Id == firstOne.Id);
            if (firstOne != null)
            {
                throw new System.Exception("Delete on failed instance still exists");
            }

            DataHelper.Cleanup(repo, context);
        }

        [TestMethod]
        public void DeleteOne()
        {
            var repo = DataHelper.getRepository();
            var context = Guid.NewGuid().ToString();
            var testData = DataHelper.CreateTestData(context, 100000);

            repo.InsertMany(testData);
            var firstOne = repo.AsQueryable().Take(1).ToList()[0];
            repo.DeleteById(firstOne.Id);
            firstOne = repo.AsQueryable().SingleOrDefault(c => c.Id == firstOne.Id);
            if(firstOne != null)
            {
                throw new System.Exception("Delete on failed instance still exists");
            }

            DataHelper.Cleanup(repo, context);
        }

        [TestMethod]
        public void UpdateOne()
        {
            var repo = DataHelper.getRepository();
            var context = Guid.NewGuid().ToString();
            var testData = DataHelper.CreateTestData(context, 1);
            repo.InsertMany(testData);
            var firstOne = repo.AsQueryable().Take(1).ToList()[0];
            firstOne.Name = "UPDATED";
            repo.ReplaceOne(firstOne);
            var updated = repo.AsQueryable().SingleOrDefault(c => c.Id == firstOne.Id);
            if (updated == null || updated.Name != "UPDATED")
            {
                throw new System.Exception("Update failed ");
            }
            DataHelper.Cleanup(repo, context);
        }
        [TestMethod]
        public void UpdateOneAsync()
        {
            var repo = DataHelper.getRepository();
            var context = Guid.NewGuid().ToString();
            var testData = DataHelper.CreateTestData(context, 1);
            repo.InsertManyAsync(testData).Wait();
            var firstOne = repo.AsQueryable().Take(1).ToList()[0];
            firstOne.Name = "UPDATED";
            repo.ReplaceOneAsync(firstOne).Wait();
            var updated = repo.AsQueryable().SingleOrDefault(c => c.Id == firstOne.Id);
            if (updated == null || updated.Name != "UPDATED" || updated.Timestamp == 0)
            {
                throw new System.Exception("Update failed ");
            }
            DataHelper.Cleanup(repo, context);
        }

     

        [TestMethod]
        public void Queryable()
        {
            var repo = DataHelper.getRepository();
            var context = Guid.NewGuid().ToString();
            var testData = DataHelper.CreateTestData(context, 100);
            repo.InsertMany(testData);
            var query = repo.AsQueryable().Where(c => c.Name.StartsWith("NAME"));
            query = query.Where(c => c.Context == context);
            var list = query.ToList();
            if (list.Count() != 100)
            {
                throw new System.Exception("retrieved different amount of data");
            }
            DataHelper.Cleanup(repo, context);
        }

        [TestMethod]
        public void MongoQuery()
        {
            var repo = DataHelper.getRepository();
            var context = Guid.NewGuid().ToString();
            var testData = DataHelper.CreateTestData(context, 100);
            repo.InsertMany(testData);
            var query = new ExpandoObject();
            query.TryAdd("Context", context);
            var result = repo.Query(System.Text.Json.JsonSerializer.Serialize(query), "");
                
            if (result.Total != 100 || result.Values.Count() != 100)
            {
                throw new System.Exception("retrieved different amount of data");
            }
            DataHelper.Cleanup(repo, context);
        }
    }
}
