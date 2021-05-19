using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestMongo.Domain.Exceptions;
using RestMongo.Test.Controller;
using RestMongo.Test.Helper;
using RestMongo.Test.Models;

namespace RestMongo.Test.Tests
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
            var controller = new TestModelReadWriteController(DataHelper.getDomainService(repo),true);
            ActionResult<string> result = controller.Update(instance.Id, instance).Result;
            var updated = repo.FindById(instance.Id);
            Assert.IsNotNull(updated);
            Assert.IsTrue(updated.Name == "TESTRESULT");

            //CONCURRENT UPDATE TEST 
            updated.Timestamp = 42 * 42;
            try
            {
                controller.Update(instance.Id, instance).Wait();
            }
            catch (AggregateException ae)
            {
                if (ae.InnerException?.GetType() == typeof(ConflictException))
                {
                    Assert.IsTrue(true);
                }
                else
                {
                    Assert.Fail("There was no ConflictException.");
                }
            }
            DataHelper.Cleanup(repo, context);
        }
        [TestMethod]
        public void Insert()
        {
            var repo = DataHelper.getRepository<TestModelFeed>();
            var context = Guid.NewGuid().ToString();
            var instance = new TestModelFeed() { Context = context };

            var controller = new TestModelReadWriteController(DataHelper.getDomainService(repo), false);
            controller.Create(instance).Wait();
            var inserted = repo.FindById(instance.Id);

            Assert.IsNotNull(inserted);

            try
            {
                controller.Create(instance).Wait();
            }
            catch (AggregateException ae)
            {
                if (ae.InnerException?.GetType() == typeof(ConflictException))
                {
                    Assert.IsTrue(true);
                }
                else
                {
                    Assert.Fail("There was no ConflictException.");
                }
            }

            DataHelper.Cleanup(repo, context);
        }
        [TestMethod]
        public void Delete()
        {
            var repo = DataHelper.getRepository<TestModelFeed>();
            var context = Guid.NewGuid().ToString();
            var instance = new TestModelFeed() { Context = context };
            repo.InsertOne(instance);
            var controller = new TestModelReadWriteController(DataHelper.getDomainService(repo), false);
            controller.Delete(instance.Id).Wait();

            try
            {
                controller.Delete("NOT THERE").Wait();
            }
            catch (AggregateException ae)
            {
                if (ae.InnerException?.GetType() == typeof(NotFoundException))
                {
                    Assert.IsTrue(true);
                }
                else
                {
                    Assert.Fail("There was no NotFoundException.");
                }
            }

            var inserted = repo.FindById(instance.Id);
            Assert.IsNull(inserted);
            DataHelper.Cleanup(repo, context);
        }
    }
}
