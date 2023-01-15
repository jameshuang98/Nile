using Microsoft.EntityFrameworkCore;
using Nile.API.Data;
using Nile.API.Entities;
using Nile.API.Repositories.Contracts;

namespace Nile.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly NileDbContext nileDbContext;

        public ProductRepository(NileDbContext NileDbContext)
        {
            nileDbContext = NileDbContext;
        }
        public async Task<IEnumerable<ProductCategory>> GetCategories()
        {
            var categories = await this.nileDbContext.ProductCategories.ToListAsync();
            return categories;
        }

        public async Task<ProductCategory> GetCategory(int id)
        {
            var category = await nileDbContext.ProductCategories.SingleOrDefaultAsync(c => c.Id == id);
            return category;
        }

        public async Task<Product> GetItem(int id)
        {
            var product = await nileDbContext.Products.FindAsync(id);
            return product;
        }

        public async Task<IEnumerable<Product>> GetItems()
        {
            var products = await this.nileDbContext.Products.ToListAsync();
            return products;
        }

        public async Task<IEnumerable<Product>> GetItemsByCategory(int id)
        {
            var products = await (from product in nileDbContext.Products
                                  where product.CategoryId == id
                                  select product).ToListAsync();
            return products;
        }
    }
}
