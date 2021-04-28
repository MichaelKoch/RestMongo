using MongoBase.Controllers;
using MongoBase.Repositories;
using MongoBase.Test.Models;

namespace MongoBase.Test.Controllers
{

    public class TestModelLocalizedReadController : LocalizedReadController<TestModelLocalized, TestModelLocalized>
    {
     
         
        public TestModelLocalizedReadController(MongoRepository<TestModelLocalized> repo) : base(repo, 10000)
        {
           
        }

    }
}
