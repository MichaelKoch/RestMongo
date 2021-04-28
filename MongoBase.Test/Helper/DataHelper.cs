using MongoBase.Interfaces;
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
            var inserted = repo.AsQueryable().Where(c => c.Context == context).ToList();
            repo.DeleteById(inserted.Select(i => i.Id).ToList());
        }
        internal static void Cleanup(MongoRepository<TestModelFeed> repo, string context)
        {
            var inserted = repo.AsQueryable().Where(c => c.Context == context).ToList();
            repo.DeleteById(inserted.Select(i => i.Id).ToList());
        }
        internal static void Cleanup(MongoRepository<TestModelLocalized> repo, string context)
        {
            var inserted = repo.AsQueryable().Where(c => c.Context == context).ToList();
            repo.DeleteById(inserted.Select(i => i.Id).ToList());
        }
        internal static void Cleanup(MongoRepository<TestModelFeedLocalized> repo, string context)
        {
            var inserted = repo.AsQueryable().Where(c => c.Context == context).ToList();
            repo.DeleteById(inserted.Select(i => i.Id).ToList());
        }
        internal static MongoRepository<TType> getRepository<TType>() where TType : IDocument
        {
            return new Repositories.MongoRepository<TType>(ConfigHelper.GetMongoConfig());
        }

        internal static IList<TestModelLocalized> CreateTestDataLocalizedModel(string context, long count, string locale = "")
        {
            var retVal = new List<TestModelLocalized>();
            for (int i = 0; i < count; i++)
            {
                retVal.Add(new TestModelLocalized() { Locale = locale, Context = context, Name = "NAME" + i, Instance = Guid.NewGuid().ToString() });
            }
            return retVal;
        }
        internal static IList<TestModel> CreateTestData(string context, long count)
        {
            var retVal = new List<TestModel>();
            for (int i = 0; i < count; i++)
            {
                retVal.Add(new TestModel() {  Context = context, Name = "NAME" + i, Instance = Guid.NewGuid().ToString() });
            }
            return retVal;
        }
        internal static IList<TestModelFeed> CreateTestDataFeed(string context, long count)
        {
            var timestamp = DateTime.UtcNow.Ticks;
            var retVal = new List<TestModelFeed>();
            for (int i = 0; i < count; i++)
            {
                retVal.Add(new TestModelFeed() {Timestamp=timestamp, Context = context, Name = "NAME" + i, Instance = Guid.NewGuid().ToString() });
            }
            return retVal;
        }

    }
}
