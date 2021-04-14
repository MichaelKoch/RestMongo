using Microsoft.AspNetCore.Mvc;
using MongoBase.Interfaces;
using Sample.Domain.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Sample.Domain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductColorController : MongoBase.Controllers.ReadController<ProductColor>
    {
        public ProductColorController(IRepository<ProductColor> repository) : base(repository)
        {

        }
    }
}
