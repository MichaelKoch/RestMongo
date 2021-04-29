using RestMongo.Controllers;
using RestMongo.Repositories;
using RestMongo.Test.Models;

namespace RestMongo.Test.Controllers
{

    public class TestModelReadController : ReadController<TestModel, TestModel>
    {


        public TestModelReadController(MongoRepository<TestModel> repo, int maxPageSize = 200) : base(repo, maxPageSize)
        {

        }

    }
}
