using RestMongo.Controllers;
using RestMongo.Repositories;
using RestMongo.Test.Models;

namespace RestMongo.Test.Controllers
{

    public class TestModelFeedController : FeedController<TestModelFeed, TestModelFeed>
    {


        public TestModelFeedController(MongoRepository<TestModelFeed> repo) : base(repo, 10000)
        {

        }

    }
}
