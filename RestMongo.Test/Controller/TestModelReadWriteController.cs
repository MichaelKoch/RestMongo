using RestMongo.Controllers;
using RestMongo.Repositories;
using RestMongo.Test.Models;

namespace RestMongo.Test.Controllers
{

    public class TestModelReadWriteController : ReadWriteController<TestModelFeed, TestModelFeed, TestModelFeed,TestModelFeed>
    {


        public TestModelReadWriteController(MongoRepository<TestModelFeed> repo,bool enableConcurrency) : base(repo, 10000,enableConcurrency)
        {

        }

    }
}
