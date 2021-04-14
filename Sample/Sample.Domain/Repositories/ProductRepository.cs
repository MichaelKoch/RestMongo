using MongoBase.Models;
using MongoDB.Driver;
using Sample.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Domain
{
    public class ProductRepository : MongoBase.Repositories.Repository<ProductColorSize>
    {
        public ProductRepository(ConnectionSettings connecttionSettings) : base(connecttionSettings)
        {}
       
    }
}
