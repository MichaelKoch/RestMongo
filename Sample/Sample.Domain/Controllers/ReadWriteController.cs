using RestMongo.Controllers;
using RestMongo.Utils;
using Sample.Domain.Models.Enities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Domain.Controllers
{
    public class ReadWriteController : ReadController#<ReadWriteEntity, ReadWriteEntity>
    {
        private ProductContext _context;
        public ReadWriteController(ProductContext context) : base(context.ReadWriteEntities, 1000)
        {
            this._context = context;
        }


    }
}
