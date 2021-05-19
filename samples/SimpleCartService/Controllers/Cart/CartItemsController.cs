//using Domain.CartService.Entities;
//using Domain.CartService.Models;
//using Microsoft.AspNetCore.Mvc;
//using RestMongo.Interfaces;
//using System.Threading.Tasks;

//namespace Domain.CartService.Controllers
//{

//    /// <summary>
//    /// 
//    /// </summary>
//    [Route("/cartitems")]
//    [ApiController]

//    public partial class CartItemsController : RestMongo.Controllers.ReadWriteController<CartItemEntity,CartItem,CartItemCreateModel,CartItemUpdateModel>
//    {


//        private IRepository<CartEntity> _cartRepo;
//        private IRepository<CartItemEntity> _cartItemRepo;



//        public CartItemsController( IRepository<CartEntity> cartRepo, IRepository<CartItemEntity> cartItemRepo
//            ) : base(cartItemRepo)
//        {
//            this._cartRepo = cartRepo;
//            this._cartItemRepo = cartItemRepo;
//        }   
//    }
//}
