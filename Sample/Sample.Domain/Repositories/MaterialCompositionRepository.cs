using MongoBase.Interfaces;
using MongoBase.Repositories;
using Sample.Domain.Models.Enities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Domain.Repositories
{
    public class MaterialCompositionRepository : MongoRepository<MaterialComposition>
    {
        private ProductContext _context;
        public MaterialCompositionRepository(IConnectionSettings connectionSettings, ProductContext context) : base(connectionSettings)
        {
            this._context = context;
        }
    }
}
