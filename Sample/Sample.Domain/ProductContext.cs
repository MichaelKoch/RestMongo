using MongoBase.Interfaces;
using Sample.Domain.Interfaces;
using Sample.Domain.Repositories;

namespace Sample.Domain
{

    public class ProductContext : IProductContext
    {
        private readonly ProductRepository _products;
        private readonly ProductColorRepository _productColors;
        private readonly ProductColorSizeRepository _productColorSizes;
       

        public ProductContext(IConnectionSettings settings)
        {
            _products = new ProductRepository(settings,this);
            _productColors = new ProductColorRepository(settings,this);
            _productColorSizes = new ProductColorSizeRepository(settings,this);
        }

        public ProductRepository Products => _products;

        public ProductColorRepository ProductColors => _productColors;

        public ProductColorSizeRepository ProductColorSizes => _productColorSizes;
       
    }
}
