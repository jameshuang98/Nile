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
            var product = await nileDbContext.Products.Include(p => p.ProductCategory).SingleOrDefaultAsync(p => p.Id == id);
            return product;
        }

        public async Task<IEnumerable<Product>> GetItems()
        {
            var products = await this.nileDbContext.Products.Include(p => p.ProductCategory).ToListAsync();
            return products;
        }

        public async Task<IEnumerable<Product>> GetItemsByCategory(int id)
        {
            var products = await this.nileDbContext.Products.Include(p => p.ProductCategory).Where(p => p.CategoryId == id).ToListAsync();
            return products;
        }
    }
}
