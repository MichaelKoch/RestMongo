using RestMongo.Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using RestMongo.Data.Abstractions.Repository.Mongo.Documents;
using RestMongo.Data.Repository;
using RestMongo.Data.Repository.Documents;
using RestMongo.Domain.Abstractions.Services;
using RestMongo.Domain.Services;

namespace RestMongo.Test.Helper
{
    public static class DataHelper
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
        internal static MongoRepository<TType> getRepository<TType>() where TType : BaseDocument
        {
            return new MongoRepository<TType>(ConfigHelper.GetMongoConfig());
        }
        
        internal static ReadWriteDomainService<TType, TType, TType, TType> getDomainService<TType>(MongoRepository<TType> repo) where TType : BaseDocument, IFeedDocument
        {
            return new(repo);
        }
        internal static ReadDomainService<TType, TType> getReadonlyDomainService<TType>(MongoRepository<TType> repo) where TType : BaseDocument
        {
            return new(repo);
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
                retVal.Add(new TestModel() { Context = context, Name = "NAME" + i, Instance = Guid.NewGuid().ToString() });
            }
            return retVal;
        }
        internal static IList<TestModelFeed> CreateTestDataFeed(string context, long count)
        {
            var timestamp = DateTime.UtcNow.Ticks;
            var retVal = new List<TestModelFeed>();
            for (int i = 0; i < count; i++)
            {
                retVal.Add(new TestModelFeed() { Timestamp = timestamp, Context = context, Name = "NAME" + i, Instance = Guid.NewGuid().ToString() });
            }
            return retVal;
        }
    }
}
