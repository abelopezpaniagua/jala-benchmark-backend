using Domain.Entities;
using Domain.Abstractions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using BenchmarkItemAPI.Dtos;
using AutoMapper;

namespace BenchmarkItemAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return Ok(await _productRepository.GetProductsAsync());
        }

        [HttpGet("{id:int}", Name = "GetProductById")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var existingProduct = await _productRepository.GetProductByIdAsync(id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            return Ok(existingProduct);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(CreationProduct creationProduct)
        {
            var product = _mapper.Map<Product>(creationProduct);
            var createdProduct = await _productRepository.CreateProductAsync(product);

            if (createdProduct == null)
            {
                return Conflict();
            }

            return CreatedAtRoute("GetProductById", new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPut("id:int")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, UpdateProduct updateProduct)
        {
            var product = _mapper.Map<Product>(updateProduct);

            if (id != product.Id)
            {
                return BadRequest();
            }

            var isUpdated = await _productRepository.UpdateProductAsync(product);

            if (!isUpdated)
            {
                return Conflict();
            }

            return Ok(product);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProductById(int id)
        {
            var existingProduct = await _productRepository.GetProductByIdAsync(id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            var isDeleted = await _productRepository.DeleteProductAsync(existingProduct);

            if (!isDeleted)
            {
                return Conflict();
            }

            return NoContent();
        }
    }
}
