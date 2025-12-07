using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Attributes;
using ServiceAbstraction;
using Shared;
using Shared.DataTransferObjects.ProductModule;
using Shared.ErrorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]")] // baseUrl/api/Products
    public class ProductsController(IServiceManager _serviceManager) : ControllerBase
    {
        // Get All Products
        //[Authorize]
        [HttpGet]
        [Cache]
        // GET: baseUrl/api/Products
        // NameAsc
        // NameDesc
        // PriceAsc
        // PriceDesc
        public async Task<ActionResult<PaginatedResult<ProductDto>>> GetAllProducts([FromQuery]ProductQueryParams queryParams)
        {
            var products = await _serviceManager.ProductService.GetAllProductsAsync(queryParams);
            return Ok(products);
        }

        // Get Product by id
        [ProducesResponseType(typeof(ProductDto), 200)]
        [ProducesResponseType(typeof(ErrorToReturn),404)]
        [HttpGet("{id:int}")]
        // GET: baseUrl/api/Products/10
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _serviceManager.ProductService.GetProductByIdAsync(id);
            return Ok(product);
        }

        // Get all Brands
        [HttpGet("brands")]
        // GET: baseUrl/api/Products
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetBrands()
        {
            var brands = await _serviceManager.ProductService.GetAllBrandsAsync();
            return Ok(brands);
        }

        // Get All Types
        [HttpGet("types")]
        // GET: baseUrl/api/Products
        public async Task<ActionResult<IEnumerable<TypeDto>>> GetTypes()
        {
            var brands = await _serviceManager.ProductService.GetAllTypesAsync();
            return Ok(brands);
        }
    }
}
