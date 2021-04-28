using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoBase.Models;
using MongoBase.Test.Helper;
using MongoBase.Test.Models;
using MongoBase.Utils;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoBase.Test
{
    [TestClass]
    public class SchemaInitilizerTest
    {

        [TestMethod]
        public void InitSchemaTest()
        {

           
            var assembly = typeof(SchemaInitilizerTest).Assembly;
            var schemaRepo = DataHelper.getRepository<DomainSchemaInfo>();
            schemaRepo.DeleteById(schemaRepo.FilterBy(x => true).ToList().Select(c => c.Id).ToList());
            SchemaInitializer.Run(ConfigHelper.GetMongoConfig(), typeof(SchemaInitilizerTest).Assembly);
            var repo = DataHelper.getRepository<TestModel>();
            var indexes = repo.Collection.Indexes.List().ToList();
           
            var schemaInfo = schemaRepo.FindOne(c => c.AssemblyName == assembly.GetName().Name);
            
            Assert.AreEqual(schemaInfo.AssemblyName, assembly.GetName().Name);
            Assert.AreEqual(schemaInfo.AssemblyVersion, assembly.GetName().Version.ToString());
            Assert.IsTrue(indexes.Count > 1);


        }
    }
}
