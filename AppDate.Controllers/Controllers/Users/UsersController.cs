using AppDate.Controllers.Controllers.Base;
using AppDate.Domain.Entities.Users;
using AppDate.Infrastructure.Persistence._Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppDate.Controllers.Controllers.Users
{
    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> AllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUser(int id)
        {
            var users = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
            return Ok(users);
        }
    }
}
