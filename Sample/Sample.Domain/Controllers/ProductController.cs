using Microsoft.AspNetCore.Mvc;
using MongoBase.Interfaces;
using Sample.Domain.ProductV1.Models;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sample.Domain.ProductNext.Controllers
{
    [Route("api/next/product")]
    [ApiController]
    [SwaggerTag("ProductNext")]
    public class ProductController : MongoBase.Controllers.Controller<Product>
    {

        [ApiExplorerSettings(IgnoreApi = true)]
        public override ActionResult<Product> Get(string id)
        {
            throw new System.NotSupportedException();
        }

        [HttpGet("{id}")]
        [SwaggerResponse(200)]
        [SwaggerResponse(404, "NOT FOUND")]
        public virtual ActionResult<Product> Get(string id,string locale)
        {
            var instance = this._repository.FindById(id);
            return instance ?? (ActionResult<Product>)NotFound();
        }

        public ProductController(IRepository<Product> repository) : base(repository)
        {

        }
    }
}
