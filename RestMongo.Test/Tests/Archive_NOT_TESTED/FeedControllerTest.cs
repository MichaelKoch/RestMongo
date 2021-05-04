//using Microsoft.AspNet.OData.Extensions;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using RestMongo.Test.Controllers;
//using RestMongo.Test.Helper;
//using RestMongo.Test.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace RestMongo.Test
//{
//    [TestClass]
//    public class FeedControllerTest
//    {

//        [TestMethod]
//        public void Delta()
//        {
//            var repo = DataHelper.getRepository<TestModelFeed>();
//            var context = Guid.NewGuid().ToString();
//            var instance = new TestModelFeed() { Context = context };
//            repo.InsertOne(instance);
//            var controller = new TestModelFeedController(repo);

//            var lastTimestamp = repo.GetMaxLastChanged();
//            var delta = ((OkObjectResult)controller.Delta(lastTimestamp).Result.Result).Value as IList<TestModelFeed>;
//            Assert.AreEqual(delta.Count(), 0);
//            instance = new TestModelFeed() { Context = context };
//            repo.InsertOne(instance);
//            delta = ((OkObjectResult)controller.Delta(lastTimestamp).Result.Result).Value as IList<TestModelFeed>;
//            var deltaInstance = delta.Where(c => c.Id == instance.Id).FirstOrDefault();
//            Assert.IsNotNull(deltaInstance);
//            DataHelper.Cleanup(repo, context);
//        }



//    }
//}
