using System.Collections.Generic;
using System.Threading.Tasks;
using RestMongo.Data.Abstractions.Extensions;
using RestMongo.Data.Abstractions.Repository;
using RestMongo.Domain.Exceptions;
using RestMongo.Domain.Services;
using SimpleCartService.Entities;
using SimpleCartService.Models.CartItem;

namespace SimpleCartService.DomainServices
{
    public class CartItemDomainService : ReadWriteDomainService<CartItemEntity, CartItem, CartItemEntity, CartItemEntity>
    {
        private readonly IRepository<CartItemEntity> _cartItemRepo;
        private readonly IRepository<CartEntity> _cartRepo;

        public CartItemDomainService(IRepository<CartItemEntity> repository,
            IRepository<CartEntity> cartRepository) : base(repository)
        {
            _cartItemRepo = repository;
            _cartRepo = cartRepository;
        }

        public async Task<CartItem> GetItemOfCartById(string id, string itemId)
        {
            if (await _cartRepo.FindByIdAsync(id) == null)
            {
                throw new NotFoundException($"Could not find cart with id '{id}'");
            }

            var item = _cartItemRepo.FindOneAsync(c => c.CartId == id && c.Id == itemId);
            return item.Transform<CartItem>();
        }

        public async Task<IList<CartItem>> GetAllItemsOfCart(string id)
        {
            if (await _cartRepo.FindByIdAsync(id) == null)
            {
                throw new NotFoundException($"Could not find cart with id '{id}'");
            }
            var items = _cartItemRepo.FilterBy(c => c.CartId == id);
            return items.Transform<List<CartItem>>();
        }

        public async Task<CartItem> CreateItem(string id, CartItemCreateModel value)
        {
            if (await _cartRepo.FindByIdAsync(id) == null)
            {
                throw new NotFoundException($"Could not find cart with id '{id}'");
            }
            var itemEntity = value.Transform<CartItemEntity>();
            itemEntity.CartId = id;
            
            return await Create(itemEntity);
        }

        public async Task UpdateItem(string id, string itemId, CartItemUpdateModel value)
        {
            if (await _cartRepo.FindByIdAsync(id) == null)
            {
                throw new NotFoundException($"Could not find cart with id '{id}'");
            }
            var itemEntity = value.Transform<CartItemEntity>();
            itemEntity.CartId = id;

            await UpdateById(itemId, itemEntity);
        }

        public async Task DeleteItem(string id, string itemId)
        {
            if (await _cartRepo.FindByIdAsync(id) == null)
            {
                throw new NotFoundException($"Could not find cart with id '{id}'");
            }

            await DeleteById(itemId);
        }
    }
}