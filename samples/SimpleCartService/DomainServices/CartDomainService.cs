using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestMongo.Data.Abstractions.Extensions;
using RestMongo.Data.Abstractions.Repository;
using RestMongo.Domain.Services;
using SimpleCartService.Entities;
using SimpleCartService.Models.Cart;
using SimpleCartService.Models.CartItem;

namespace SimpleCartService.DomainServices
{
    public class CartDomainService: ReadWriteDomainService<CartEntity, CartDto, CartCreateModel, CartUpdateModel> 
    {
        private readonly IRepository<CartItemEntity> _cartItemRepo;

        public CartDomainService(IRepository<CartEntity> repository, IRepository<CartItemEntity> cartItemRepo) : base(repository)
        {
            _cartItemRepo = cartItemRepo;
        }

        public override async Task<IList<CartDto>> LoadRelations(IList<CartDto> values, IList<string> relations)
        {
            List<Task> waitfor = new List<Task>();
            if (relations.Contains("Items"))
            {
                relations.Remove("Items");
                waitfor.Add(loadItems(values));
            }
            Task.WaitAll(waitfor.ToArray());
            if (relations.Count > 0)
            {
                throw new KeyNotFoundException();
            }
            return values;
        }


        private async Task<bool> loadItems(IList<CartDto> values)
        {
            var cartIds = values.Select(c => c.Id).ToList();
            var items = await _cartItemRepo.FilterByAsync(c => cartIds.Contains(c.CartId));

            foreach (var cart in values)
            {
                cart.Items = items.Where(c => c.CartId == cart.Id).ToList().Transform<List<CartItemCreateModel>>();
            }
            return true;
        }
    }
}
