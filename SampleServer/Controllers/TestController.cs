using Microsoft.AspNetCore.Mvc;
using MongoBase.Interfaces;
using SampleServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SampleServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : MongoBase.Controllers.Controller<TestModel>
    {
       public TestController(IRepository<TestModel> repository):base(repository)
       {

       }

      


    }
}
