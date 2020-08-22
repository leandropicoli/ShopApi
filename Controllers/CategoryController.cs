using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
    [Route("v1/categories")]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 30)]
        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
        {
            var categories = await context.Categories.AsNoTracking().ToListAsync();

            return Ok(categories);
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Category>> GetById(int id, [FromServices] DataContext context)
        {
            var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return Ok(category);
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "empolyee")]
        public async Task<ActionResult<Category>> Post([FromBody] Category category, [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Categories.Add(category);
                await context.SaveChangesAsync();
                return Ok(category);
            }
            catch
            {
                return BadRequest(new { message = "Error creating new category." });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "empolyee")]
        public async Task<ActionResult<Category>> Put(int id, [FromBody] Category category, [FromServices] DataContext context)
        {
            if (id != category.Id)
                return NotFound(new { message = "Not found category." });
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                context.Entry<Category>(category).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(category);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "This category is already updated." });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occurred when trying to update category." });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "empolyee")]
        public async Task<ActionResult<Category>> Delete(int id, [FromServices] DataContext context)
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound(new { message = "Category not found." });

            try
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                return Ok(new { message = "The category was deleted." });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occurred when trying to delete category." });
            }
        }
    }
}