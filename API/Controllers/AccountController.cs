using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        // private readonly DataContext context;
        private readonly ItokenService tokenservice;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ItokenService tokenservice, IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            // DataContext context,
            _mapper = mapper;
            this.tokenservice = tokenservice;
            // this.context = context;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDTo register)
        {
            if (await UserExists(register.Username)) return BadRequest("Username is taken");

            var user = _mapper.Map<AppUser>(register);

            // using var hmac = new HMACSHA512();
            // var user = new AppUser
            // {
            user.UserName = register.Username.ToLower();
            // user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(register.password));
            // user.PasswordSalt = hmac.Key;
            // };
            // context.Users.Add(user);
            // await context.SaveChangesAsync();
            var result = await _userManager.CreateAsync(user,register.password);
            if(!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user,"Member");

            if(!roleResult.Succeeded) return BadRequest(result.Errors);

            return new UserDto
            {
                username = user.UserName,
                token = await tokenservice.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };

        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var User = await _userManager.Users
            .Include(p => p.photos)
            .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (User == null) return Unauthorized("Invalid userName");

            var result = await _signInManager
                .CheckPasswordSignInAsync(User, loginDto.Password,false);
            
            if (!result.Succeeded) return Unauthorized();
            // using var hmac = new HMACSHA512(User.PasswordSalt);
            // var ComputedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            // for (int i = 0; i < ComputedHash.Length; i++)
            // {
            //     if (ComputedHash[i] != User.PasswordHash[i]) return Unauthorized("Invalid password");
            // }
            return new UserDto
            {
                username = User.UserName,
                token = await tokenservice.CreateToken(User),
                PhotoUrl = User.photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = User.KnownAs,
                Gender = User.Gender
            };
        }

        private async Task<bool> UserExists(string Username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == Username.ToLower());
        }
    }
}