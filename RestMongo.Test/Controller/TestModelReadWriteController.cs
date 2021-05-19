using RestMongo.Data.Repository;
using RestMongo.Domain.Abstractions.Services;
using RestMongo.Domain.Services;
using RestMongo.Test.Models;
using RestMongo.Web.Controllers;

namespace RestMongo.Test.Controller
{
    public class
        TestModelReadWriteController : ReadWriteController<TestModelFeed, TestModelFeed, TestModelFeed>
    {
        public TestModelReadWriteController(
            IReadWriteDomainService<TestModelFeed, TestModelFeed, TestModelFeed> domainService,
            bool enableConcurrency) : base(domainService, 10000, enableConcurrency)
        {
        }
    }
}