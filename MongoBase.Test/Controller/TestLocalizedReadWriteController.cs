using MongoBase.Controllers;
using MongoBase.Repositories;
using MongoBase.Test.Models;

namespace MongoBase.Test.Controllers
{

    public class TestLocalizedReadWriteController : LocalizedReadWriteController<TestModelFeedLocalized, TestModelFeedLocalized>
    {
     
         
        public TestLocalizedReadWriteController(MongoRepository<TestModelFeedLocalized> repo) : base(repo, 10000)
        {
           
        }

    }
}
