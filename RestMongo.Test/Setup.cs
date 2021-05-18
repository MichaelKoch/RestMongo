using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mongo2Go;
using RestMongo.Test.Helper;

namespace RestMongo.Test
{
    [TestClass]
    public class Setup
    {

        public static MongoDbRunner mongoInstance = null;
        [AssemblyInitialize]
        public static void Prepare(TestContext context)
        {

            mongoInstance = Mongo2Go.MongoDbRunner.Start(singleNodeReplSet: true);
            ConfigHelper._config.ConnectionString = mongoInstance.ConnectionString;
        }
        [AssemblyCleanup]
        public static void Dispose()
        {
            if(mongoInstance != null)
            { mongoInstance.Dispose(); }
      
        }
    }
}