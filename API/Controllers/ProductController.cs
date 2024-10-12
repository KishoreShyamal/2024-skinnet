using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("get-products")]
        public async Task<IReadOnlyList<Product>> Getproducts(string? brand, string? type, string? sort, bool isDescending)
        {
            return await _productRepository.GetProductsAsync(brand, type, sort, isDescending);
        }

        [HttpGet("{id}")] //api/product/2
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();
            return product;            
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProducts(Product product)
        {
            _productRepository.AddProduct(product);
            if (await _productRepository.SaveChangesAsync())
            {
                return CreatedAtAction("GetProduct", new {id = product.Id}, product);
            }
            return BadRequest("Getting some proble with creating the product");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !IsProductExist(id))
            {
                return BadRequest("Cannot update this");
            }
            
            _productRepository.UpdateProduct(product);
            if (await _productRepository.SaveChangesAsync())
            {
                return NoContent();
            }

            return BadRequest("Getting some problem while updating the product");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();
            _productRepository.DeleteProduct(product);
            if (await _productRepository.SaveChangesAsync())
            {
                return NoContent();
            }
            return BadRequest("Getting some problem while deleting the product");
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetProductBrands()
        {
            return Ok(await _productRepository.GetBrandsAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetProducttypes()
        {
            return Ok(await _productRepository.GetTypesAsync());
        }

        private bool IsProductExist(int id)
        {
            return _productRepository.IsProductExist(id);
        }
    }
}
