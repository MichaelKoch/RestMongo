using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestMongo.Test.Controllers;
using RestMongo.Test.Helper;
using RestMongo.Test.Models;
using System;

namespace RestMongo.Test
{
    [TestClass]
    public class ReadWriteControllerTest
    {
        [TestMethod]
        public void Update()
        {
            var repo = DataHelper.getRepository<TestModelFeed>();
            var context = Guid.NewGuid().ToString();
            var instance = new TestModelFeed() { Context = context };
            repo.InsertOne(instance);
            instance = repo.FindById(instance.Id);
            instance.Name = "TESTRESULT";
            var controller = new TestModelReadWriteController(repo,true);
            ActionResult<string> result = controller.Update(instance.Id, instance).Result;
            var updated = repo.FindById(instance.Id);
            Assert.IsNotNull(updated);
            Assert.IsTrue(updated.Name == "TESTRESULT");

            //CONCURRENT UPDATE TEST 
            updated.Timestamp = 42 * 42;
            result = controller.Update(instance.Id, instance).Result;
            Assert.IsTrue(result.Result is ConflictObjectResult);
            DataHelper.Cleanup(repo, context);
        }
        [TestMethod]
        public void Insert()
        {
            var repo = DataHelper.getRepository<TestModelFeed>();
            var context = Guid.NewGuid().ToString();
            var instance = new TestModelFeed() { Context = context };

            var controller = new TestModelReadWriteController(repo,false);
            controller.Create(instance).Wait();
            var inserted = repo.FindById(instance.Id);

            Assert.IsNotNull(inserted);
            ActionResult<TestModelFeed> result = controller.Create(instance).Result;
            Assert.IsTrue(result.Result is ConflictObjectResult);
            DataHelper.Cleanup(repo, context);

        }
        [TestMethod]
        public void Delete()
        {
            var repo = DataHelper.getRepository<TestModelFeed>();
            var context = Guid.NewGuid().ToString();
            var instance = new TestModelFeed() { Context = context };
            repo.InsertOne(instance);
            var controller = new TestModelReadWriteController(repo,false);
            controller.Delete(instance.Id).Wait();
            controller.Delete("NOT THERE").Wait();
            var inserted = repo.FindById(instance.Id);
            Assert.IsNull(inserted);
            DataHelper.Cleanup(repo, context);
        }
    }
}
