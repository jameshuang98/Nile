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

        public Task<ProductCategory> GetCategory(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetItem(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetItems()
        {
            var products = await this.nileDbContext.Products.ToListAsync();
            return products;
        }
    }
}
