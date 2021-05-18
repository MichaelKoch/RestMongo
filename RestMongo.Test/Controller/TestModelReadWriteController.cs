using RestMongo.Data.Repository;
using RestMongo.Test.Models;
using RestMongo.Web.Controllers;

namespace RestMongo.Test.Controller
{

    public class TestModelReadWriteController : ReadWriteController<TestModelFeed, TestModelFeed, TestModelFeed,TestModelFeed>
    {


        public TestModelReadWriteController(MongoRepository<TestModelFeed> repo,bool enableConcurrency) : base(repo, 10000,enableConcurrency)
        {

        }

    }
}
