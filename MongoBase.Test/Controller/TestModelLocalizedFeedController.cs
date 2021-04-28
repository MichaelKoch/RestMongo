using MongoBase.Controllers;
using MongoBase.Repositories;
using MongoBase.Test.Models;

namespace MongoBase.Test.Controllers
{

    public class TestModelLocalizedFeedController : LocalizedFeedController<TestModelFeedLocalized, TestModelFeedLocalized>
    {
     
         
        public TestModelLocalizedFeedController(MongoRepository<TestModelFeedLocalized> repo,int maxPageSize=100) : base(repo, maxPageSize)
        {
           
        }

    }
}
