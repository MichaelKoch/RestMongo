using MongoBase.Interfaces;
using MongoBase.Models;
using MongoDB.Driver;
using Sample.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Domain.Repositories
{
    public class WTFRepository : MongoBase.Repositories.Repository<WTFModel>
    {
        protected ProductContext _productContext;
        public WTFRepository(
                IConnectionSettings connecttionSettings,
                ProductContext productContext) : base(connecttionSettings)
        {
            this._productContext = productContext;
        }
       
    }
}
