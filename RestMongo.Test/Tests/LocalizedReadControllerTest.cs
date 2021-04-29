using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestMongo.Test.Controllers;
using RestMongo.Test.Helper;
using RestMongo.Test.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace RestMongo.Test
{
    [TestClass]
    public class LocalizedReadControllerTest
    {
        [TestMethod]
        public void Query()
        {
            var repo = DataHelper.getRepository<TestModelLocalized>();
            var context = Guid.NewGuid().ToString();
            var instance = new TestModelLocalized() { Context = context, Locale = "de-DE" };
            repo.InsertOne(instance);
            var controller = new TestModelLocalizedReadController(repo);
            var query = new ExpandoObject();
            query.TryAdd("Context", context);
            var actionResult = controller.Query(query, "de-DE", "Timestamp", "Test").Result;
            var pagedResult = ((OkObjectResult)actionResult.Result).Value as PagedResultModel<TestModelLocalized>;
            query = new ExpandoObject();
            query.TryAdd("Context", context);
            actionResult = controller.Query(query, "en-GB", "Timestamp", "Test").Result;
            var pagedResultEN = ((OkObjectResult)actionResult.Result).Value as PagedResultModel<TestModelLocalized>;
           
            
            Assert.IsTrue(pagedResultEN.Values.Count == 0);
            Assert.IsTrue(pagedResultEN.Top == 0);
            Assert.IsTrue(pagedResultEN.Total == 0);
            Assert.IsTrue(pagedResult.Values.Count == 1);
            Assert.IsTrue(pagedResult.Top == 1);
            Assert.IsTrue(pagedResult.Total == 1);
            DataHelper.Cleanup(repo, context);
        }

        [TestMethod]
        public void GetByODataQuery()
        {
            var repo = DataHelper.getRepository<TestModelLocalized>();
            var context = Guid.NewGuid().ToString();
            var testDataDE = DataHelper.CreateTestDataLocalizedModel(context, 250, "de-DE");
            repo.InsertManyAsync(testDataDE).Wait();
            var testDataEN = DataHelper.CreateTestDataLocalizedModel(context, 10, "en-GB");
            repo.InsertManyAsync(testDataEN).Wait();
            var controller = new TestModelLocalizedReadController(repo);
            var call = controller.Get("de-DE", 200, 0, $"Context eq '{context}'").Result;
            var valueDE = call.Value;


            call = controller.Get("en-GB", 200, 0, $"Context eq '{context}'").Result;
            var valueEN = call.Value;
            Assert.IsTrue(valueEN.Total == 10);
            Assert.IsTrue(valueEN.Top == 200);
            Assert.IsTrue(valueEN.Values.Count() == 10);

            Assert.IsTrue(valueDE.Total == 250);
            Assert.IsTrue(valueDE.Top == 200);
            Assert.IsTrue(valueDE.Values.Count() == 200);


            DataHelper.Cleanup(repo, context);
        }


        [TestMethod]
        public void GetByID()
        {
            var repo = DataHelper.getRepository<TestModelLocalized>();
            var context = Guid.NewGuid().ToString();
            var instance = new TestModelLocalized() { Context = context, Locale = "de-DE" };
            repo.InsertOne(instance);
            TestModelLocalizedReadController controller = new TestModelLocalizedReadController(repo);

            var actionResult = controller.Get(instance.Id, "de-DE");
            var result = actionResult.Result.Value;
            Assert.IsNotNull(result);

            actionResult = controller.Get(instance.Id, "en-GB");
            result = actionResult.Result.Value;
            Assert.IsNull(result);

            DataHelper.Cleanup(repo, context);
        }

    }
}
