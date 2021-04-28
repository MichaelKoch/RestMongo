using MongoBase.Controllers;
using MongoBase.Repositories;
using MongoBase.Test.Models;

namespace MongoBase.Test.Controllers
{

    public class TestModelReadController : ReadController<TestModel,TestModel>
    {
     
         
        public TestModelReadController(MongoRepository<TestModel> repo,int maxPageSize=200) : base(repo, maxPageSize)
        {
           
        }

    }
}
