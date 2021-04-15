using Microsoft.AspNetCore.Mvc;
using MongoBase.Interfaces;
using Sample.Domain.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Sample.Domain.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WTFController : MongoBase.Controllers.Controller<WTFModel>
    {
        public WTFController(ProductContext context) : base(context.WTFS)
        {

        }
    }
}
