using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoBase;
using MongoBase.Attributes;
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
