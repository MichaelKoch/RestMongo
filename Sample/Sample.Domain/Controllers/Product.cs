using Microsoft.AspNetCore.Mvc;
using MongoBase.Interfaces;
using Sample.Domain.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Sample.Domain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductColorSizeController : MongoBase.Controllers.Controller<ProductColorSize>
    {
        public ProductColorSizeController(IRepository<ProductColorSize> repository) : base(repository)
        {

        }
    }
}
