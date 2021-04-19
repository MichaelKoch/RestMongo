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
        
        public override async Task<IList<Product>> LoadRelations(IList<Product> values,IList<string> relations)
        {
            List<Task> waitfor = new List<Task>();
            List<string> materialNumbers = values.Select(v => v.MaterialNumber).ToList();
            relations = relations.Select(c => c.ToLower()).ToList();
            List<ProductColor> colors = null;
            List<ProductColorSize> variants = null;
            if (relations.Contains("colors"))
            {
                waitfor.Add(
                            _productContext.ProductColors
                                .GetByProductNumber(materialNumbers)
                                .ContinueWith(r=>colors = r.Result));
            }
            if (relations.Contains("variants"))
            {
                waitfor.Add(
                         _productContext.ProductColorSizes
                             .GetByProductNumber(materialNumbers)
                             .ContinueWith(r => variants = r.Result));
            };

            Task.WaitAll(waitfor.ToArray());
            foreach(var v in values)
            {
                if(colors != null)
                {
                    v.Colors = colors.Where(c => c.MaterialNumber == v.MaterialNumber)
                                     .OrderBy(c => c.Color)
                                     .ToList();
                }
                if (variants != null)
                {
                    v.Variants = variants.Where(c => c.MaterialNumber == v.MaterialNumber)
                                     .OrderBy(c => c.ColorSize)
                                     .ToList();
                }
            }
            return values;
        }
    }
}
