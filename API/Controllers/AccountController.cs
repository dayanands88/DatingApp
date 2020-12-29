using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext context;
        private readonly ItokenService tokenservice;
        private readonly IMapper _mapper;
        public AccountController(DataContext context, ItokenService tokenservice, IMapper mapper)
        {
            _mapper = mapper;
            this.tokenservice = tokenservice;
            this.context = context;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDTo register)
        {
            if (await UserExists(register.Username)) return BadRequest("Username is taken");

            var user = _mapper.Map<AppUser>(register);

            using var hmac = new HMACSHA512();
            // var user = new AppUser
            // {
                user.UserName = register.Username.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(register.password));
                user.PasswordSalt = hmac.Key;
            // };
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return new UserDto
            {
                username = user.UserName,
                token = tokenservice.CreateToken(user),
                KnownAs =user.KnownAs,
                Gender = user.Gender
            };

        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var User = await context.Users
            .Include(p => p.photos)
            .SingleOrDefaultAsync(x => x.UserName == loginDto.Username);
            if (User == null) return Unauthorized("Invalid userName");
            using var hmac = new HMACSHA512(User.PasswordSalt);
            var ComputedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i = 0; i < ComputedHash.Length; i++)
            {
                if (ComputedHash[i] != User.PasswordHash[i]) return Unauthorized("Invalid password");
            }
            return new UserDto
            {
                username = User.UserName,
                token = tokenservice.CreateToken(User),
                PhotoUrl = User.photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = User.KnownAs,
                Gender = User.Gender
            };
        }

        private async Task<bool> UserExists(string Username)
        {
            return await context.Users.AnyAsync(x => x.UserName == Username.ToLower());
        }
    }
}