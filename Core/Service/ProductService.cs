using DomainLayer.Contracts;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Service.Specifications;
using Shared;
using DomainLayer.Exceptions;
using DomainLayer.Models.ProductModule;
using Shared.DataTransferObjects.ProductModule;

namespace Service
{
    public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper) : IProductService
    {
        public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
        {
            var Repo = _unitOfWork.GetRepository<ProductBrand, int>();
            var Brands = await Repo.GetAllAsync();
            var BrandsDto = _mapper.Map<IEnumerable<ProductBrand>, IEnumerable<BrandDto>>(Brands);
            return BrandsDto;
        }

        public async Task<PaginatedResult<ProductDto>> GetAllProductsAsync(ProductQueryParams queryParams)
        {
            var specification = new ProductWithBrandAndTypeSpecifications(queryParams);
            var products = await _unitOfWork.GetRepository<Product, int>().GetAllAsync(specification);
            var productsDto = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
            var productCount = products.Count();
            var countSpec = new ProductCountSpecification(queryParams);
            var totalCount = await _unitOfWork.GetRepository<Product, int>().CountAsync(countSpec);
            return new PaginatedResult<ProductDto>(queryParams.PageIndex, productCount, totalCount, productsDto);
        }

        public async Task<IEnumerable<TypeDto>> GetAllTypesAsync()
        {
            var types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();

            return _mapper.Map<IEnumerable<ProductType>, IEnumerable<TypeDto>>(types);
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var specification = new ProductWithBrandAndTypeSpecifications(id);
            var product = await _unitOfWork.GetRepository<Product, int>().GetbyIdAsync(specification);
            if (product is null)
                throw new ProductNotFoundException(id);
            return _mapper.Map<Product, ProductDto>(product);
        }
    }
}
