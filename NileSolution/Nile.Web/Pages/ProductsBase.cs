using Microsoft.AspNetCore.Components;
using Nile.Models.Dtos;
using Nile.Web.Services.Contracts;

namespace Nile.Web.Pages
{
    public class ProductsBase:ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Products = await ProductService.GetItems();
        }
    }
}
