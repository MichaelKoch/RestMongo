using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestMongo.Data.Attributes;
using RestMongo.Test.Models;

namespace RestMongo.Test.Tests
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
