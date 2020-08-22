using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
    [Route("v1/products")]
    public class ProductController : ControllerBase
    {

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
        {
            var products = await context.Products.Include(x => x.Category).AsNoTracking().ToListAsync();
            return products;
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Product>> GetById([FromServices] DataContext context, int id)
        {
            var product = await context.Products.Include(x => x.Category).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return product;
        }

        [HttpGet] //products/categories/1
        [Route("categories/{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Product>>> GetByCategory([FromServices] DataContext context, int categoryId)
        {
            var products = await context.Products.Include(x => x.Category).AsNoTracking()
                    .Where(y => y.CategoryId == categoryId).ToListAsync();
            return products;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "empolyee")]
        public async Task<ActionResult<Product>> Post([FromServices] DataContext context, [FromBody] Product product)
        {
            if (ModelState.IsValid)
            {
                context.Products.Add(product);
                await context.SaveChangesAsync();
                return Ok(product);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}