using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.DTO;
using ProductsAPI.Models;

namespace ProductsAPI.Controllers
{


    // localhost:5000/api/products
    
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {

        private readonly ProductsContext _context;

        public ProductsController(ProductsContext productsContext)
        {
            _context = productsContext;
        }

        // localhost:5000/api/products => GET
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.
            Products.
            Where(i => i.IsActive).
            Select(p => ProductToDTO(p)).
            ToListAsync();

            return Ok(products);
        }

        // localhost:5000/api/products/1 => GET
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.
            Products.Where(i => i.ProductId == id).
            Select(p => ProductToDTO(p)).
            FirstOrDefaultAsync();

            if(product == null)
            {
                return NotFound();
            }

            return Ok(product);

        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if(id != product.ProductId)
            {
                return BadRequest();
            }

            var _product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);

            if(product == null)
            {
                return NotFound();
            }

            _product.ProductName = product.ProductName;
            _product.Price = product.Price;
            _product.IsActive = product.IsActive;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception)
            {
                return NotFound();
            }
            
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            if(id == null)
            {
                return BadRequest();
            }

            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);

            if(product == null)
            {
                return NotFound();
            }

             _context.Products.Remove(product);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NoContent();
        }

        private static ProductDTO ProductToDTO(Product product)
        {
            var entity = new ProductDTO();
            if(product != null)
            {
                entity.ProductId = product.ProductId;
                entity.ProductName = product.ProductName;
                entity.Price = product.Price;
            }
            return entity;
        }
    }
}