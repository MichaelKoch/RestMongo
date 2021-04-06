using Microsoft.AspNetCore.Mvc;
using MongoBase.Interfaces;
using SampleServer.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SampleServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Test1Controller : MongoBase.Controllers.Controller<Test1Model>
    {
       public Test1Controller(IRepository<Test1Model> repository):base(repository)
       {

       }
    }
}
