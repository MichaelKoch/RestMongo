using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Query.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.UriParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoBase.Test.Helper;
using MongoBase.Test.Models;
using Moq;
using Sample.Domain.Controllers;
using System;

namespace MongoBase.Test
{
    [TestClass]
    public class ReadController
    {

    


        [TestMethod]
        public void GetById()
        {
            var repo = DataHelper.getRepository();
            var context = Guid.NewGuid().ToString();
            var instance = new TestModel() { Context = context };
            repo.InsertOne(instance);
            TestReadController controller = new TestReadController(repo);
            var inserted = controller.Get(instance.Id).Result;
            if (inserted == null)
            {
                throw new Exception("insert failed . can't retrieve instance by id");
            }
            DataHelper.Cleanup(repo, context);
        }

        [TestMethod]
        public void GetByODataQuery()
        {
            var repo = DataHelper.getRepository();
            var context = Guid.NewGuid().ToString();
            var testData = DataHelper.CreateTestData(context, 250);
        

            repo.InsertManyAsync(testData).Wait();
            TestReadController controller = new TestReadController(repo);

           
            var call = controller.Get(200, 0, $"Context eq '{context}'").Result;
            var value = call.Value;
            if (value.Total != 250)
            {
                throw new Exception("total not matching");
            }
            if (value.Top != 200)
            {
                throw new Exception("total not matching");
            }
            if (value.Values.Count != 200)
            {
                throw new Exception("value count not matching");
            }
            DataHelper.Cleanup(repo, context);
        }
    }
}
