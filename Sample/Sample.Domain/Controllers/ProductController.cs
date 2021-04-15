using Microsoft.AspNetCore.Mvc;
using MongoBase.Interfaces;
using Sample.Domain.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Sample.Domain.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : MongoBase.Controllers.ReadController<Product>
    {
        public ProductsController(ProductContext context) : base(context.Products)
        {

        }
    }
}
