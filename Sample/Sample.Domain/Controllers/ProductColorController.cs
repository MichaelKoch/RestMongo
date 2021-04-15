using Microsoft.AspNetCore.Mvc;
using MongoBase.Interfaces;
using Sample.Domain.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Sample.Domain.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductColorsController : MongoBase.Controllers.ReadController<ProductColor>
    {
        public ProductColorsController(IRepository<ProductColor> repository) : base(repository)
        {

        }
    }
}
