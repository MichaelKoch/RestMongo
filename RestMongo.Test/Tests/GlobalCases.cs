using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestMongo.Test.Helper;

namespace RestMongo.Test.Tests
{
    [TestClass]
    public class GlobalCases
    {
        [TestMethod]
        public void EmptyOdataFilter()
        {
            var list = DataHelper.CreateTestDataFeed("NO", 10);
            //ODataQueryHelper.Apply<TestModelFeed>("", list.AsQueryable());
        }
    }
}
