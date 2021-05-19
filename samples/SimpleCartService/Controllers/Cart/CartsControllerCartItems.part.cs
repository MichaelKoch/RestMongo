using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleCartService.Models.CartItem;
using Swashbuckle.AspNetCore.Annotations;

namespace SimpleCartService.Controllers.Cart
{
    public partial class CartsController
    {
        [HttpGet("{id}/items/{itemId}")]
        [SwaggerResponse(200)]
        [SwaggerResponse(404, "CONFLICT", typeof(ProblemDetails))]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation("Get CartDto item by ID", OperationId = "CartGetItemById")]

        public virtual async Task<ActionResult<CartItem>> GetItem(string id, string itemId)
        {
            var retVal = await _cartItemService.GetItemOfCartById(id, itemId);
            return Ok(retVal);
        }

        [HttpGet("{id}/items")]
        [SwaggerResponse(200)]
        [SwaggerResponse(404, "NOT FOUND", typeof(ProblemDetails))]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation("Get CartDto items by ID", OperationId = "CartGetAllItems")]
        public virtual async Task<ActionResult<IList<CartItem>>> GetAllItems(string id)
        {
            var retVal = await _cartItemService.GetAllItemsOfCart(id);
            return Ok(retVal);
        }

        [HttpPost("{id}/items/")]
        [SwaggerResponse(200)]
        [SwaggerResponse(409, "CONFLICT", typeof(ProblemDetails))]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation("update cart item by id", OperationId = "CartCreateItem")]
        public virtual async Task<ActionResult> CreateItem(string id, [FromBody] CartItemCreateModel value)
        {
            var retVal = await _cartItemService.CreateItem(id, value);
            return Ok(retVal);
        }

        [HttpPut("{id}/items/{itemId}")]
        [SwaggerResponse(204, "NO CONTENT")]
        [SwaggerResponse(404, "NOT FOUND", typeof(ProblemDetails))]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation("update cart item by id", OperationId = "CartUpdateItemById")]
        public virtual async Task<ActionResult<string>> UpdateItem(string id, string itemId, [FromBody] CartItemUpdateModel value)
        {
            await _cartItemService.UpdateItem(id, itemId, value);
            return NoContent();
        }

        [HttpDelete("{id}/items/{itemId}")]
        [SwaggerResponse(204)]
        [SwaggerResponse(404, "CONFLICT", typeof(ProblemDetails))]
        [SwaggerResponse(400)]
        [SwaggerResponse(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation("delete cart item by id", OperationId = "CartDeleteItemById")]
        public virtual async Task<ActionResult<CartItem>> DeleteItem(string id, string itemId)
        {
            await _cartItemService.DeleteItem(id, itemId);
            return NoContent();
        }
    }
}
