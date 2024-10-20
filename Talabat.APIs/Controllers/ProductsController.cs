using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{
    
    public class ProductsController : APIBaseController
    {
        private readonly IGenericRepository<Product> _productRepo;
		private readonly IMapper _mapper;
        private readonly IGenericRepository<ProductType> _typeRepo;
        private readonly IGenericRepository<ProductBrand> _brandRepo;

        public ProductsController(IGenericRepository<Product> ProductRepo, IMapper mapper, IGenericRepository<ProductType> typeRepo, IGenericRepository<ProductBrand> brandRepo)
        {
            _productRepo = ProductRepo;
			_mapper = mapper;
            _typeRepo = typeRepo;
            _brandRepo = brandRepo;
        }

        // Get All Products
        [HttpGet] // BaseUrl/api/Products   --> Get
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var Spec = new ProductWithBrandAndTypeSpecifications();

            var Products = await _productRepo.GetAllWithSpecAsync(Spec);

            var MappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(Products);

            //OkObjectResult result = new OkObjectResult(Products);
            //return result;

            return Ok(MappedProducts);

        }

        // Get Product By Id
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProductById(int id)
        {
            var Spec = new ProductWithBrandAndTypeSpecifications(id);

            var product = await _productRepo.GetByIdWithSpecAsync(Spec);

            if (product is null) return NotFound(new ApiResponse(404));

            var MappedProduct = _mapper.Map<Product, ProductToReturnDto>(product);

            return Ok(MappedProduct);
        }

        // Get All Types
        [HttpGet("Types")]
        // baseUrl/api/products/Typess

        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var Types = await _typeRepo.GetAllAsync();

            return Ok(Types);

        }


        // Get All Brands
        [HttpGet("Brands")]
        // baseurl/api/products/Brands
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _brandRepo.GetAllAsync();

            return Ok(brands);
        }


    }
}
