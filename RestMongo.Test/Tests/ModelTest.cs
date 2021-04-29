using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestMongo;
using RestMongo.Attributes;
using RestMongo.Interfaces;
using RestMongo.Models;
using RestMongo.Repositories;
using RestMongo.Test.Helper;
using RestMongo.Test.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace RestMongo.Test
{
    [TestClass]
    public class ModelTest
    {



        [TestMethod]
        public void IsQueryableTest()
        {
            var type = typeof(TestModelFeed);
            var notQueryable = type.GetProperty("NotQueryable");
            var queryable = type.GetProperty("Timestamp");
            Assert.IsFalse(IsQueryableAttribute.IsAssignedTo(notQueryable));
            Assert.IsTrue(IsQueryableAttribute.IsAssignedTo(queryable));
        }

    }
}
