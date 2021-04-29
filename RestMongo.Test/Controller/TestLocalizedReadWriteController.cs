using RestMongo.Controllers;
using RestMongo.Repositories;
using RestMongo.Test.Models;

namespace RestMongo.Test.Controllers
{

    public class TestLocalizedReadWriteController : LocalizedReadWriteController<TestModelFeedLocalized, TestModelFeedLocalized>
    {


        public TestLocalizedReadWriteController(MongoRepository<TestModelFeedLocalized> repo) : base(repo, 10000)
        {

        }

    }
}
