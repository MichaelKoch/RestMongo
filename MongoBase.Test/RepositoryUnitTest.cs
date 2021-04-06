using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoBase;
using MongoBase.Models;
using MongoBase.Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MongoBase.Test
{
    [TestClass]
    public class RepositoryUnitTest
    {

      

        private IList<TestModel> CreateTestData(string name,long count)
        {
            var retVal = new List<TestModel>();
            for (int i = 0; i < count; i++)
            {
                retVal.Add(new TestModel() { Name = name,Instance=i });
            }
            return retVal;
        }

        [TestMethod]
        public void InsertOne()
        {
            Repositories.Repository<TestModel> repo = new Repositories.Repository<TestModel>(new ConnectionSettings() { 
                ConnectionString = "mongodb://localhost",
                DatabaseName ="test"
            });
            var context = Guid.NewGuid().ToString();
            repo.InsertOne(new TestModel() { Name = context });
            var inserted = repo.AsQueryable().FirstOrDefault(c => c.Name == context);
            if(inserted == null)
            {
                throw new Exception("insert failed . can't retrieve instance by id");
            }
            repo.DeleteById(inserted.Id);
        }

        [TestMethod]
        public void InsertMany()
        {
            Repositories.Repository<TestModel> repo = new Repositories.Repository<TestModel>(new ConnectionSettings()
            {
                ConnectionString = "mongodb://localhost",
                DatabaseName = "test"
            });
            var context = Guid.NewGuid().ToString();
            var testData = CreateTestData(context, 100000);
            repo.InsertMany(testData);
            var inserted = repo.AsQueryable().Where(c => c.Name == context).ToList();

            if (inserted.Count != testData.Count)
            {
                throw new System.Exception("Inserted items not matched");
            }
            repo.DeleteById(inserted.Select(i => i.Id).ToList());
        }

        [TestMethod]
        public void DeleteOne()
        {
            Repositories.Repository<TestModel> repo = new Repositories.Repository<TestModel>(new ConnectionSettings()
            {
                ConnectionString = "mongodb://localhost",
                DatabaseName = "test"
            });

            var context = Guid.NewGuid().ToString();
            var testData = CreateTestData(context, 100000);

            repo.InsertMany(testData);
            var firstOne = repo.AsQueryable().Take(1).ToList()[0];
            repo.DeleteById(firstOne.Id);
            firstOne = repo.AsQueryable().SingleOrDefault(c => c.Id == firstOne.Id);
            if(firstOne != null)
            {
                throw new System.Exception("Delete on failed instance still exists");
            }
            var inserted = repo.AsQueryable().Where(c => c.Name == context).ToList();
            repo.DeleteById(inserted.Select(i => i.Id).ToList());
        }
    }
}
