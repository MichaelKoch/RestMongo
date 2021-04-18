using Microsoft.AspNetCore.Mvc;
using MongoBase.Interfaces;
using MongoDB.Driver;
using Sample.Domain.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Domain.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : MongoBase.Controllers.ReadController<Product>
    {
        private ProductContext _context;
        public override ActionResult<PagedResultModel<Product>> Get([FromQuery(Name = "$top")] int top = 200, [FromQuery(Name = "$skip")] int skip = 0, [FromQuery(Name = "$filter")] string filter = "")
        {
            var result =  base.Get(top, skip, filter).Value;
            var matnrs = result.Values.Select(m => m.MaterialNumber);
            var query = Builders<ProductColorSize>.Filter;
            var pcs = this._context.ProductColorSizes.AsQueryable().Where(c => matnrs.Contains(c.MaterialNumber)).ToList();

            Parallel.ForEach(result.Values,p =>
            {
                p.Variants = pcs.Where(c => c.MaterialNumber == p.MaterialNumber).OrderBy(c => c.ColorSize).ToList();
            });
          
            return result;

        }
        public ProductsController(ProductContext context) : base(context.Products)
        {
            this._context = context;
            
        }
    }
}
