using Microsoft.EntityFrameworkCore;
using Nile.API.Data;
using Nile.API.Entities;
using Nile.API.Repositories.Contracts;
using Nile.Models.Dtos;

namespace Nile.API.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly NileDbContext nileDbContext;

        public ShoppingCartRepository(NileDbContext nileDbContext)
        {
            this.nileDbContext = nileDbContext;
        }

        // checks if item already exists in the user's shopping cart
        private async Task<bool> CartItemExists(int cardId, int productId)
        {
            return await this.nileDbContext.CartItems.AnyAsync(c => c.CartId == cardId && c.ProductId == productId);
        }
        public async Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            if (await CartItemExists(cartItemToAddDto.CartId, cartItemToAddDto.ProductId) == false)
            {
                // use LINQ query get appropriate item from Products table with ProductId
                var item = await (from product in this.nileDbContext.Products
                                  where product.Id == cartItemToAddDto.ProductId
                                  select new CartItem
                                  {
                                      CartId = cartItemToAddDto.CartId,
                                      ProductId = product.Id,
                                      Qty = cartItemToAddDto.Qty
                                  }).SingleOrDefaultAsync();


                // if the item returned from LINQ query is not null, we want to add that product to the CartItems database table
                if (item != null)
                {
                    var result = await this.nileDbContext.CartItems.AddAsync(item);

                    // using the AddAsync method above, we must also call the SaveChangesAsync method
                    await this.nileDbContext.SaveChangesAsync();

                    // then we can return the entity that has been added to the CartItems database table
                    return result.Entity;
                }
            }

            // return null if item is not added to database
            return null;

        }

        public Task<CartItem> DeleteItem(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<CartItem> GetItem(int id)
        {
            return await (from cart in this.nileDbContext.Carts
                          join cartItem in this.nileDbContext.CartItems
                          on cart.Id equals cartItem.Id
                          where cartItem.Id == id
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              Qty = cartItem.Qty,
                              CartId = cartItem.CartId
                          }).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<CartItem>> GetItems(int userId)
        {
            return await (from cart in this.nileDbContext.Carts
                          join cartItem in this.nileDbContext.CartItems
                          on cart.Id equals cartItem.CartId
                          where cart.UserId == userId
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              Qty = cartItem.Qty,
                              CartId = cartItem.CartId
                          }).ToListAsync();
        }

        public Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            throw new NotImplementedException();
        }
    }
}
