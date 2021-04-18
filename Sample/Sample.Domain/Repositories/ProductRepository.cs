using MongoBase.Interfaces;
using MongoBase.Models;
using MongoDB.Driver;
using Sample.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sample.Domain.Repositories
{
    public class ProductRepository : MongoBase.Repositories.Repository<Product>
    {
        protected ProductContext _productContext;
        public ProductRepository(
                IConnectionSettings connecttionSettings,
                ProductContext productContext) : base(connecttionSettings)
        {
            this._productContext = productContext;
        }

        public override IEnumerable<Product> FilterBy(Expression<Func<Product, bool>> filterExpression)
        {
            var result = base.FilterBy(filterExpression).ToList();
            var matnrs = result.Select(m => m.MaterialNumber);
            var query = Builders<ProductColorSize>.Filter;
            var pcs = this._productContext.ProductColorSizes.Query(query.All("MaterialNumber", matnrs).ToString(),"");
            return result;
        }

    }
}
