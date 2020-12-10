using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;

        private readonly IUserRepository _userRespository;
        public UsersController(IUserRepository userRespository, IMapper mapper)
        {
            _mapper = mapper;
            _userRespository = userRespository;

        }
        [HttpGet]

        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            // var user = await _userRespository.GetUserAsync();
            // var UsersToReturn =_mapper.Map<IEnumerable<MemberDto>>(user);
            // return Ok(UsersToReturn);
            var Users= await _userRespository.GetMembersAsync();
            return Ok(Users);

        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
           return await _userRespository.GetMemberAsync(username);
            // return _mapper.Map<MemberDto>(user);
            // return await _userRespository.GetUserByUserName(username);

        }
    }
}