using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestMongo.Test.Controllers;
using RestMongo.Test.Helper;
using RestMongo.Test.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace RestMongo.Test
{
    [TestClass]
    public class ReadControllerTest
    {




        [TestMethod]
        public void GetByQuery()
        {
            var repo = DataHelper.getRepository<TestModel>();
            var context = Guid.NewGuid().ToString();
            var instance = new TestModel() { Context = context };
            repo.InsertOne(instance);
            TestModelReadController controller = new TestModelReadController(repo);
            var query = new ExpandoObject();
            query.TryAdd("Context", context);
            var actionResult = controller.Query(query, "Timestamp", "Test").Result;
            var pagedResult = ((OkObjectResult)actionResult.Result).Value as PagedResultModel<TestModel>;
            Assert.IsTrue(pagedResult.Values.Count == 1);
            Assert.IsTrue(pagedResult.Top == 1);
            Assert.IsTrue(pagedResult.Total == 1);

            DataHelper.Cleanup(repo, context);
        }

        [TestMethod]
        public void GetByQueryPageSizeExeeded()
        {
            var repo = DataHelper.getRepository<TestModel>();
            var context = Guid.NewGuid().ToString();
            var testData = DataHelper.CreateTestData(context, 250);
            repo.InsertManyAsync(testData).Wait();
            TestModelReadController controller = new TestModelReadController(repo, 200);
            var query = new ExpandoObject();
            query.TryAdd("Context", context);
            var actionResult = controller.Query(query, "Timestamp", "Test").Result;
            var pagedResult = (ObjectResult)actionResult.Result;
            Assert.IsTrue(pagedResult.StatusCode == 412);

            DataHelper.Cleanup(repo, context);
        }
        [TestMethod]
        public void GetByID()
        {
            var repo = DataHelper.getRepository<TestModel>();
            var context = Guid.NewGuid().ToString();
            var instance = new TestModel() { Context = context };
            repo.InsertOne(instance);
            TestModelReadController controller = new TestModelReadController(repo);

            var actionResult = controller.Get(instance.Id);
            var result = actionResult.Result.Value;
            Assert.IsNotNull(result);

            actionResult = controller.Get("NOT THERE");
            result = actionResult.Result.Value;
            Assert.IsNull(result);
            DataHelper.Cleanup(repo, context);
        }

        [TestMethod]
        public void GetByODataQuery()
        {
            var repo = DataHelper.getRepository<TestModel>();
            var context = Guid.NewGuid().ToString();
            var testData = DataHelper.CreateTestData(context, 250);
            repo.InsertManyAsync(testData).Wait();
            TestModelReadController controller = new TestModelReadController(repo);
            var call = controller.Get(200, 0, $"Context eq '{context}'").Result;
            var value = call.Value;
            Assert.IsTrue(value.Total == 250);
            Assert.IsTrue(value.Top == 200);
            Assert.IsTrue(value.Values.Count == 200);
            DataHelper.Cleanup(repo, context);
        }
        [TestMethod]
        public void GetByODataQuerySkip()
        {
            var repo = DataHelper.getRepository<TestModel>();
            var context = Guid.NewGuid().ToString();
            var testData = DataHelper.CreateTestData(context, 250);
            repo.InsertManyAsync(testData).Wait();
            TestModelReadController controller = new TestModelReadController(repo);
            var call = controller.Get(10, 100, $"Context eq '{context}'").Result;
            var value = call.Value;
            Assert.IsTrue(value.Total == 250);
            Assert.IsTrue(value.Top == 10);
            Assert.IsTrue(value.Skip == 100);
            Assert.IsTrue(value.Values.Count == 10);
            DataHelper.Cleanup(repo, context);
        }

        [TestMethod]
        public void GetByODataQueryPageSizeExeeded()
        {
            var repo = DataHelper.getRepository<TestModel>();
            var context = Guid.NewGuid().ToString();
            var testData = DataHelper.CreateTestData(context, 250);
            repo.InsertManyAsync(testData).Wait();
            TestModelReadController controller = new TestModelReadController(repo);
            var actionResult = controller.Get(999, 0, $"Context eq '{context}'").Result;
            var pagedResult = (ObjectResult)actionResult.Result;
            Assert.IsTrue(pagedResult.StatusCode == 412);
        }
    }
}
