using Microsoft.AspNetCore.Components;
using Nile.Models.Dtos;

namespace Nile.Web.Pages
{
    public class DisplayProductsBase : ComponentBase
    {
        [Parameter]
        public IEnumerable<ProductDto> Products { get; set; }
    }
}
