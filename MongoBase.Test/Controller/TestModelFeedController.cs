using MongoBase.Controllers;
using MongoBase.Repositories;
using MongoBase.Test.Models;

namespace MongoBase.Test.Controllers
{

    public class TestModelFeedController : FeedController<TestModelFeed, TestModelFeed>
    {
     
         
        public TestModelFeedController(MongoRepository<TestModelFeed> repo) : base(repo, 10000)
        {
           
        }

    }
}
