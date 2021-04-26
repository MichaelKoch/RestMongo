﻿using MongoBase.Interfaces;
using MongoBase.Repositories;
using Sample.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Domain.Repositories
{
    public class MaterialTextRepository : MongoRepository<MaterialText>
    {
        private ProductContext _context;
        public MaterialTextRepository(IConnectionSettings connectionSettings, ProductContext context) : base(connectionSettings)
        {
            this._context = context;

        }
    }
}