using MongoBase.Repositories;
using MongoBase.Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoBase.Test.Helper
{
  public static  class DataHelper
    {
        internal static void Cleanup(MongoRepository<TestModel> repo, string context)
        {
            var inserted = repo.AsQueryable().Where(c => c.Name == context).ToList();
            repo.DeleteById(inserted.Select(i => i.Id).ToList());
        }
        internal static MongoRepository<TestModel> getRepository()
        {
            return new Repositories.MongoRepository<TestModel>(ConfigHelper.GetMongoConfig());
        }


        internal static IList<TestModel> CreateTestData(string context, long count)
        {
            var retVal = new List<TestModel>();
            for (int i = 0; i < count; i++)
            {
                retVal.Add(new TestModel() { Context = context, Name = "NAME" + i, Instance = Guid.NewGuid().ToString() });
            }
            return retVal;
        }

    }
}
