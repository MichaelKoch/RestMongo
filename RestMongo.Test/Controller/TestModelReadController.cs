using RestMongo.Data.Repository;
using RestMongo.Test.Models;
using RestMongo.Web.Controllers;

namespace RestMongo.Test.Controller
{

    public class TestModelReadController : ReadController<TestModel, TestModel>
    {


        public TestModelReadController(MongoRepository<TestModel> repo, int maxPageSize = 200) : base(repo, maxPageSize)
        {

        }

    }
}
