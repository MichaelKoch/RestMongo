using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoBase.Test.Controllers;
using MongoBase.Test.Helper;
using MongoBase.Test.Models;
using System;

namespace MongoBase.Test
{
    [TestClass]
    public class LocalizedEntityControllerTest
    {
        [TestMethod]
        public void Update()
        {
            var repo = DataHelper.getRepository<TestModelFeedLocalized>();
            var context = Guid.NewGuid().ToString();
            var instance = new TestModelFeedLocalized() { Context = context };
            repo.InsertOne(instance);
            instance = repo.FindById(instance.Id);
            instance.Name = "TESTRESULT";
            var controller = new TestLocalizedReadWriteController(repo);
            ActionResult<string> result = controller.Put(instance.Id, instance);
            var updated = repo.FindById(instance.Id);
            Assert.IsNotNull(updated);
            Assert.IsTrue(updated.Name == "TESTRESULT");

            //CONCURRENT UPDATE TEST 
            updated.Timestamp = 42 * 42;
            result = controller.Put(instance.Id, instance);
            Assert.IsTrue(result.Result is ConflictObjectResult);
            DataHelper.Cleanup(repo, context);
        }
        [TestMethod]
        public void Insert()
        {
            var repo = DataHelper.getRepository<TestModelFeedLocalized>();
            var context = Guid.NewGuid().ToString();
            var instance = new TestModelFeedLocalized() { Context = context };

            var controller = new TestLocalizedReadWriteController(repo);
            controller.Post(instance);
            var inserted = repo.FindById(instance.Id);

            Assert.IsNotNull(inserted);
            ActionResult<string> result = controller.Post(instance);
            Assert.IsTrue(result.Result is ConflictObjectResult);
            DataHelper.Cleanup(repo, context);

        }
        [TestMethod]
        public void Delete()
        {
            var repo = DataHelper.getRepository<TestModelFeedLocalized>();
            var context = Guid.NewGuid().ToString();
            var instance = new TestModelFeedLocalized() { Context = context };
            var controller = new TestLocalizedReadWriteController(repo);
            controller.Delete(instance.Id);
            var inserted = repo.FindById(instance.Id);
            Assert.IsNull(inserted);
            DataHelper.Cleanup(repo, context);
        }

    }
}
