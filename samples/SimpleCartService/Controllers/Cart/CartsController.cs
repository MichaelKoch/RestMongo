using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestMongo.Data.Abstractions.Repository;
using RestMongo.Web.Controllers;
using SimpleCartService.Entities;
using SimpleCartService.Models.Cart;

namespace SimpleCartService.Controllers.Cart
{

    /// <summary>
    /// 
    /// </summary>
    [Route("/carts")]
    [ApiController]

    public partial class CartsController : ReadWriteController<CartEntity, CartDto, CartCreateModel, CartUpdateModel>
    {
        private IRepository<CartEntity> _cartRepo;
        private IRepository<CartItemEntity> _cartItemRepo;

        public override Task<ActionResult<CartDto>> Get(string id, [FromQuery(Name = "$expand")] string expand = "")
        {
            return base.Get(id, expand);
        }

        public CartsController(IRepository<CartEntity> cartRepo, IRepository<CartItemEntity> cartItemRepo) : base(cartRepo)
        {
            this._cartRepo = cartRepo;
            this._cartItemRepo = cartItemRepo;
        }
    }
}
