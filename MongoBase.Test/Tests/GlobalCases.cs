using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoBase;
using MongoBase.Interfaces;
using MongoBase.Models;
using MongoBase.Repositories;
using MongoBase.Test.Helper;
using MongoBase.Test.Models;
using MongoBase.Utils;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace MongoBase.Test
{
    [TestClass]
    public class GlobalCases
    {


        [TestMethod]
        public void EmptyOdataFilter()
        {
            var list = DataHelper.CreateTestDataFeed("NO", 10);
            ODataQueryHelper.Apply<TestModelFeed>("", list.AsQueryable());

        
        }


    }
}
