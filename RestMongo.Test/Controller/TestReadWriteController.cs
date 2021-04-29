using RestMongo.Controllers;
using RestMongo.Repositories;
using RestMongo.Test.Models;

namespace RestMongo.Test.Controllers
{

    public class TestReadWriteController : ReadWriteController<TestModelFeed, TestModelFeed>
    {


        public TestReadWriteController(MongoRepository<TestModelFeed> repo) : base(repo, 10000)
        {

        }

    }
}
