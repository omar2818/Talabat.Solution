using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.Product_Specs;

namespace Talabat.APIs.Controllers
{
	public class ProductController : BaseAPIController
	{
		private readonly IGenericRepository<Product> _productRepo;
		private readonly IMapper _mapper;

		public ProductController(IGenericRepository<Product> productRepo, IMapper mapper)
        {
			_productRepo = productRepo;
			_mapper = mapper;
		}

		// /api/Products
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ProductToReturnDTO>>> GetProducts()
		{
			var spec = new ProductWithBrandAndCategorySpecifications();
			var products = await _productRepo.GetAllWithSpecAsync(spec);

			return Ok(_mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDTO>>(products));
		}

		// /api/Products/10
		[HttpGet("{id}")]
		public async Task<ActionResult<ProductToReturnDTO>> GetProduct(int id)
		{
			var spec = new ProductWithBrandAndCategorySpecifications(id);
			var product = await _productRepo.GetWithSpecAsync(spec);

			if(product is null)
			{
				return NotFound(new {Message = "Not Found", StatusCode = 404}); // 404
			}

			return Ok(_mapper.Map<Product, ProductToReturnDTO>(product)); // 200
		}
	}
}
