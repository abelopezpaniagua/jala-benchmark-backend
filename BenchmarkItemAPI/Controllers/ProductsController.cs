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
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] FilterParams filters)
        {
            return Ok(await _productService.GetProducts(filters));
        }

        [HttpGet("{id:int}", Name = "GetProductById")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var existingProduct = await _productService.GetProductById(id);

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
            var createdProduct = await _productService.CreateProduct(product);

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

            var isUpdated = await _productService.UpdateProduct(product);

            if (!isUpdated)
            {
                return Conflict();
            }

            return Ok(product);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProductById(int id)
        {
            var existingProduct = await _productService.GetProductById(id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            var isDeleted = await _productService.DeleteProduct(existingProduct);

            if (!isDeleted)
            {
                return Conflict();
            }

            return NoContent();
        }
    }
}
