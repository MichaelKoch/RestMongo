using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestMongo.Domain.Abstractions.Services;
using RestMongo.Web.Controllers;
using SimpleCartService.DomainServices;
using SimpleCartService.Entities;
using SimpleCartService.Models.Cart;

namespace SimpleCartService.Controllers.Cart
{

    /// <summary>
    /// 
    /// </summary>
    [Route("/carts")]
    [ApiController]

    public partial class CartsController : ReadWriteController<CartDto, CartCreateModel, CartUpdateModel>
    {
        private readonly CartItemDomainService _cartItemService;

        public CartsController(CartDomainService cartService, CartItemDomainService cartItemService) : base(cartService)
        {
            _cartItemService = cartItemService;
        }
    }
}
