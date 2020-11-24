using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext context;
        private readonly ItokenService tokenservice;
        public AccountController(DataContext context, ItokenService tokenservice)
        {
            this.tokenservice = tokenservice;
            this.context = context;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDTo register)
        {
            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName = register.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(register.password)),
                PasswordSalt = hmac.Key
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return new UserDto
            {
                Username = user.UserName,
                Token = tokenservice.CreateToken(user)
            };

        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var User = await context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username);
            if (User == null) return Unauthorized("Invalid userName");
            using var hmac = new HMACSHA512(User.PasswordSalt);
            var ComputedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i = 0; i < ComputedHash.Length; i++)
            {
                if (ComputedHash[i] != User.PasswordHash[i]) return Unauthorized("Invalid password");
            }
            return new UserDto
            {
                Username = User.UserName,
                Token = tokenservice.CreateToken(User)
            };
        }

        private async Task<bool> UserExists(string Username)
        {
            return await context.Users.AnyAsync(x => x.UserName == Username.ToLower());
        }
    }
}