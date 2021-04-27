using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoBase;
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
    public class GlobalClasses
    {

       

        [TestMethod]
        public void PagedResultModel()
        {
            var resultModel = new PagedResultModel<TestModel>();
            resultModel.Values = DataHelper.CreateTestData("c", 100);
            resultModel.Skip = 0;
            resultModel.Top = 100;
            resultModel.Total = 100;

            if(resultModel.Skip !=0)
            {
                throw new System.Exception("skip not matching");
            }
            if (resultModel.Total != 100)
            {
                throw new System.Exception("Total not matching");
            }
            if (resultModel.Top != 100)
            {
                throw new System.Exception("Top not matching");
            }
            if (resultModel.Values.Count != 100)
            {
                throw new System.Exception("value count matching");
            }
        }
       
    }
}
