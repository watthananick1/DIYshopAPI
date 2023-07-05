using DIYshopAPI.Data;
using DIYshopAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DIYshopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserdbContext _context;
        public UserController(UserdbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }
            var user = await _context.Users.ToListAsync();
            return user == null ? BadRequest("User Not Found.") : Ok(user);
        }

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            var user = await _context.Users.FindAsync(id);
            return user == null ? BadRequest("User Not Found.") : Ok(user);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = passwordHash;
            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, UserUpdate userUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _context.Users.FindAsync(id);
            var dataUser = user;
            if (user == null) return BadRequest();

            user.UserName = userUpdate.UserName ?? dataUser.UserName;
            user.Firstname = userUpdate.Firstname ?? dataUser.Firstname;
            user.Lastname = userUpdate.Lastname ?? dataUser.Lastname;
            user.Status = userUpdate.Status ?? dataUser.Status;
            user.Email = userUpdate.Email ?? dataUser.Email;
            user.PhoneNumber = userUpdate.PhoneNumber ?? dataUser.PhoneNumber;
            user.Proflieimg = userUpdate.Proflieimg ?? null;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
         
        }

        [HttpPut("ChangePassword/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword(int id, UserChangePassword userChangePassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null) return BadRequest();
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(userChangePassword.Password);
            user.Password = passwordHash;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
