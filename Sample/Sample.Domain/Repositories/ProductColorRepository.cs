using MongoBase.Interfaces;
using MongoBase.Models;
using MongoDB.Driver;
using Sample.Domain.Models;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace Sample.Domain.Repositories
{
    public class ProductColorRepository : MongoBase.Repositories.Repository<ProductColor>
    {
        protected ProductContext _productContext;

        public async Task<List<ProductColor>> GetByProductNumber(List<string> materialNumbers)
        {
          return AsQueryable().Where(i => materialNumbers.Contains(i.MaterialNumber)).ToList();
        }
        public ProductColorRepository(
                IConnectionSettings connecttionSettings,
                ProductContext productContext) : base(connecttionSettings)
        {
            _productContext = productContext; 
        }

    }
}
