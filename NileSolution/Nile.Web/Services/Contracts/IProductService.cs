﻿using Nile.Models.Dtos;

namespace Nile.Web.Services.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetItems();
    }
}
