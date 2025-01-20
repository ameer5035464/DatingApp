using AppDate.Application.Abstraction.Models.UsersDTOs;
using AppDate.Application.Abstraction.Services;
using AppDate.Controllers.Controllers.Base;
using AppDate.Domain.Entities.Users;
using AppDate.Infrastructure.Persistence._Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace AppDate.Controllers.Controllers.Account
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context , ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterUserDto registerUser)
        {
            if (await UserExists(registerUser.UserName)) return BadRequest();

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerUser.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerUser.Password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userDto = new UserDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user),
            };

            return Ok(userDto);
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginUserDto loginUser)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginUser.UserName);

            if (user is null)
                return Unauthorized();


            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginUser.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized();
            }

            var userDto = new UserDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user),
            };

            return Ok(userDto);

        }

        private async Task<bool> UserExists(string userName)
        {
            return await _context.Users.AnyAsync(u => u.UserName == userName);
        }
    }
}
