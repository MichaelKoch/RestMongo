using MongoBase.Controllers;
using Sample.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Domain.Controllers
{
   public class CollectionMaterialController:FeedController<CollectionMaterial>
   {
        private ProductContext _context;
        public CollectionMaterialController(ProductContext  context):base(context.CollectionMaterials)
        {
            this._context = context;
            
        }
    }
}
