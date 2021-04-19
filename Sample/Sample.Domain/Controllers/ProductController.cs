using Microsoft.AspNetCore.Mvc;
using MongoBase.Interfaces;
using MongoDB.Driver;
using Sample.Domain.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Domain.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : MongoBase.Controllers.ReadController<Product>
    {
        private ProductContext _context;

        
        public ProductsController(ProductContext context) : base(context.Products)
        {
            this._context = context;
            
        }
    }
}
