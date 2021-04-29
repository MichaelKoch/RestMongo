
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestMongo.Interfaces;
using RestMongo.Models;
using RestMongo.Test.Helper;
using RestMongo.Test.Models;
using RestMongo.Utils;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestMongo.Test
{
    [TestClass]
    public class BootstrapTest
    {

        [TestMethod]
        public void IApplicationBuilder()
        {
            var app = Mock.Of<IApplicationBuilder>();
            app.AddRestMongo();
        }

        [TestMethod]
        public void IServiceCollection()
        {

            var mongoConfig = ConfigHelper.GetMongoConfig();
            var inMemorySettings = new Dictionary<string, string> {
                    {"mongo:ConnectionString",mongoConfig.ConnectionString},
                    {"mongo:DatabaseName", mongoConfig.DatabaseName}
            };
            IConfiguration configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection(inMemorySettings)
                            .Build();

            var collection = new ServiceCollection();

            collection.AddControllers();

            collection.AddMvc(options => options.EnableEndpointRouting = false);
            collection.AddRestMongo<BootstrapTest>(configuration);

            var provider = collection.BuildServiceProvider();
            var mongoRep = provider.GetRequiredService<IRepository<TestModel>>();
            Assert.IsNotNull(mongoRep);

        }
    }
}
