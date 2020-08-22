using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Shop.Services;

namespace Shop.Controllers
{
    [Route("v1/users")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<List<User>>> Get([FromServices] DataContext context)
        {
            var users = await context.Users.AsNoTracking().ToListAsync();
            return users;
        }

        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Post([FromServices] DataContext context, [FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                //User always be created as employee
                user.Role = "employee";

                context.Users.Add(user);
                await context.SaveChangesAsync();

                user.Password = "";

                return Ok(user);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Error creating new user." });

            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate([FromServices] DataContext context, [FromBody] User userRequest)
        {
            var user = await context.Users.AsNoTracking()
                .Where(x => x.Username == userRequest.Username && x.Password == userRequest.Password)
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound(new { message = "User or password incorrect." });

            var token = TokenService.GenerateToken(user);

            user.Password = "";

            return new
            {
                user = user,
                token = token
            };
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<User>> Put(
            [FromServices] DataContext context,
            int id,
            [FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != user.Id)
                return NotFound(new { message = "User not found" });

            try
            {
                context.Entry(user).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return user;
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Error updating user" });

            }
        }
    }
}