using Domain.Entities;
using Domain.Abstractions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace BenchmarkItemAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return Ok(await _productRepository.GetProductsAsync());
        }

        [HttpGet("{id:int}", Name = "GetProductById")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var existingProduct = await _productRepository.GetProductByIdAsync(id)

            if (existingProduct == null)
            {
                return NotFound();
            }

            return Ok(existingProduct);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            var createdProduct = await _productRepository.CreateProductAsync(product);

            if (createdProduct == null)
            {
                return Conflict();
            }

            return CreatedAtRoute("GetProductById", new { id = product.Id }, createdProduct);
        }

        [HttpPut("id:int")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, Product product)
        {
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
