using RestMongo.Data.Repository;
using RestMongo.Domain.Abstractions.Services;
using RestMongo.Domain.Services;
using RestMongo.Test.Models;
using RestMongo.Web.Controllers;

namespace RestMongo.Test.Controller
{

    public class TestModelReadController : ReadController<TestModel>
    {
        public TestModelReadController(IReadDomainService<TestModel> domainService, int maxPageSize = 200) : base(domainService, maxPageSize)
        {
        }
    }
}
