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
//    public class LocalizedFeedControllerTest
//    {




//        [TestMethod]
//        public void Delta()
//        {
//            var repo = DataHelper.getRepository<TestModelFeedLocalized>();
//            var context = Guid.NewGuid().ToString();
//            var instance = new TestModelFeedLocalized() { Context = context, Locale = "de-DE" };
//            repo.InsertOne(instance);
//            var controller = new TestModelLocalizedFeedController(repo, 1000);

//            var lastTimestamp = repo.GetMaxLastChanged();
//            var delta = ((OkObjectResult)controller.Delta(lastTimestamp).Result.Result).Value as IList<TestModelFeedLocalized>;
//            Assert.AreEqual(delta.Count(), 0);
//            instance = new TestModelFeedLocalized() { Context = context, Locale = "de-DE" };
//            repo.InsertOne(instance);
//            delta = ((OkObjectResult)controller.Delta(lastTimestamp).Result.Result).Value as IList<TestModelFeedLocalized>;
//            var deltaInstance = delta.Where(c => c.Id == instance.Id).FirstOrDefault();
//            Assert.IsNotNull(deltaInstance);
//            DataHelper.Cleanup(repo, context);
//        }

//        [TestMethod]
//        public void DeltaPageSizeExeeded()
//        {
//            var repo = DataHelper.getRepository<TestModelFeedLocalized>();
//            var controller = new TestModelLocalizedFeedController(repo, 10);
//            ObjectResult reponse = ((ObjectResult)controller.Delta(0, 100).Result.Result);
//            Assert.IsTrue(reponse.StatusCode == 412);
//        }

//    }
//}
