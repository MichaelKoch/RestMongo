using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestMongo;
using RestMongo.Interfaces;
using RestMongo.Models;
using RestMongo.Repositories;
using RestMongo.Test.Helper;
using RestMongo.Test.Models;
using RestMongo.Utils;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace RestMongo.Test
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
