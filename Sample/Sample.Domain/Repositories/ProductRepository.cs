﻿using MongoBase.Interfaces;
using MongoBase.Models;
using MongoDB.Driver;
using Sample.Domain.Models;
using System.Collections.Generic;
using System.Linq;
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
       
    }
}