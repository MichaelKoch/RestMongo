using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestMongo;
using RestMongo.Data.Abstractions.Extensions;
using SimpleCartService.Models.CartItem;

namespace SimpleCartService.Controllers.Cart
{
    public partial class CartsController
    {
        protected override async Task<IList<Models.Cart.CartDto>> LoadRelations(IList<Models.Cart.CartDto> values, IList<string> relations)
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
       

        private async Task<bool> loadItems(IList<Models.Cart.CartDto> values)
        {
            var cartIds  = values.Select(c => c.Id).ToList();
            var items = await _cartItemRepo.FilterByAsync(c => cartIds.Contains(c.CartId));
           
            foreach (var cart in values)
            {
                cart.Items = items.Where(c=>c.CartId== cart.Id).ToList().Transform<List<CartItemCreateModel>>();
            }
            return true;
        }
    }
}
