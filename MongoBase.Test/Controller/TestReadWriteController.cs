using MongoBase.Controllers;
using MongoBase.Repositories;
using MongoBase.Test.Models;

namespace MongoBase.Test.Controllers
{

    public class TestReadWriteController : ReadWriteController<TestModelFeed, TestModelFeed>
    {
     
         
        public TestReadWriteController(MongoRepository<TestModelFeed> repo) : base(repo, 10000)
        {
           
        }

    }
}
