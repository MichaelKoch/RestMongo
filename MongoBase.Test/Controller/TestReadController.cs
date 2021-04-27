using Microsoft.AspNetCore.Mvc;
using MongoBase.Controllers;
using MongoBase.Repositories;
using MongoBase.Test.Models;
using MongoBase.Utils;
using Sample.Domain.Models.Enities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Domain.Controllers
{

    public class TestReadController : ReadController<TestModel,TestModel>
    {
     
         
        public TestReadController(MongoRepository<TestModel> repo) : base(repo, 10000)
        {
           
        }

    }
}
