using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nile.API.Extensions;
using Nile.API.Repositories.Contracts;
using Nile.Models.Dtos;

namespace Nile.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly IProductRepository productRepository;

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
            this.productRepository = productRepository;
        }

        [HttpGet]
        [Route("{userId}/GetItems")]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetItems(int userId)
        {
            try
            {
                var cartItems = await this.shoppingCartRepository.GetItems(userId);

                // return 204 status code if shopping cart has no items
                if (cartItems == null)
                {
                    return NoContent();
                }

                var products = await this.productRepository.GetItems();

                if (products == null)
                {
                    throw new Exception("No products exist in the system");
                }

                var cartItemsDto = cartItems.ConvertToDto(products);

                // return 200 status code
                return Ok(cartItemsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CartItemDto>> GetItem(int id)
        {
            try
            {
                var cartItem = await this.shoppingCartRepository.GetItem(id);
                if (cartItem == null)
                {
                    return NotFound();
                }

                var product = await productRepository.GetItem(cartItem.ProductId);
                if (product == null)
                {
                    return NotFound();
                }

                var cartItemDto = cartItem.ConvertToDto(product);

                return Ok(cartItemDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // Post request to add item to shopping cart
        [HttpPost]
        public async Task<ActionResult<CartItemDto>> PostItem([FromBody] CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                var newCartItem = await this.shoppingCartRepository.AddItem(cartItemToAddDto);
                if (newCartItem == null)
                {
                    return NoContent();
                }

                var product = await productRepository.GetItem(newCartItem.ProductId);
                if (product == null)
                {
                    throw new Exception($"Something went wrong when attempting to retrieve product (productId:({newCartItem.ProductId})");
                }

                var newCartItemDto = newCartItem.ConvertToDto(product);

                // It is standard practice for a Post action method to return the location of the resource where the newly added item can be found
                // The location will be returned in the header of the HTTP response
                return CreatedAtAction(nameof(GetItem), new { id = newCartItemDto.Id }, newCartItemDto);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CartItemDto>> DeleteItem(int id)
        {
            try
            {
                var cartItem = await this.shoppingCartRepository.DeleteItem(id);
                if (cartItem == null)
                {
                    return NotFound();
                }

                var product = await this.productRepository.GetItem(cartItem.ProductId);

                if (product == null)
                {
                    return NotFound();
                }

                var cartItemDto = cartItem.ConvertToDto(product);

                return Ok(cartItemDto);



            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<CartItemDto>> UpdateQty (int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            try
            {
                var cartItem = await this.shoppingCartRepository.UpdateQty(id, cartItemQtyUpdateDto);
                if (cartItem == null)
                {
                    return NotFound();
                }
                var product = await productRepository.GetItem(cartItem.ProductId);

                var cartItemDto = cartItem.ConvertToDto(product);

                return Ok(cartItemDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
