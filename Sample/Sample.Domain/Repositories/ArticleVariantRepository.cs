using RestMongo.Interfaces;
using RestMongo.Repositories;
using Sample.Domain.Models.Enities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Domain.Repositories
{
    public class ArticleVariantRepository : MongoRepository<ArticleVariant>
    {
        private ProductContext _context;
        public ArticleVariantRepository(IConnectionSettings connectionSettings, ProductContext context) : base(connectionSettings)
        {
            this._context = context;

        }


    }
}
