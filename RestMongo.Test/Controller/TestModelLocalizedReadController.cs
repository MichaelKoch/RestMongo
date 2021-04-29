using RestMongo.Controllers;
using RestMongo.Repositories;
using RestMongo.Test.Models;

namespace RestMongo.Test.Controllers
{

    public class TestModelLocalizedReadController : LocalizedReadController<TestModelLocalized, TestModelLocalized>
    {


        public TestModelLocalizedReadController(MongoRepository<TestModelLocalized> repo) : base(repo, 10000)
        {

        }

    }
}
