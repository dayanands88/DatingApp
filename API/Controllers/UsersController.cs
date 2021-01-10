using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPhotoService _photservice;

        private readonly IUserRepository _userRespository;
        public UsersController(IUserRepository userRespository, IMapper mapper,
         IPhotoService photservice)
        {
            _photservice = photservice;
            _mapper = mapper;
            _userRespository = userRespository;

        }

        // [Authorize(Roles = "Admin")]
        [HttpGet]

        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery]UserParams userParams)
        {
            // var user = await _userRespository.GetUserAsync();
            // var UsersToReturn =_mapper.Map<IEnumerable<MemberDto>>(user);
            // return Ok(UsersToReturn);
            // var user = await _userRespository.GetUserByIdAsync(Convert.ToInt32(User.GetUsername()));
            userParams.CurrentUsername = User.GetUsername();

            if (string.IsNullOrEmpty(userParams.Gender))
                userParams.Gender = userParams.Gender == "male" ? "female" : "male";
            var Users = await _userRespository.GetMembersAsync(userParams);
            Response.AddPaginationHeader(Users.CurrentPage, Users.PageSize, 
                    Users.TotalCount, Users.TotalPages);
            return Ok(Users);

        }

        [Authorize(Roles = "Member")]
        [HttpGet("{username}",Name ="GetUser")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _userRespository.GetMemberAsync(username);
            // return _mapper.Map<MemberDto>(user);
            // return await _userRespository.GetUserByUserName(username);

        }
        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            // var username = User.GetUsername();
            var user = await _userRespository.GetUserByUserName(User.GetUsername());

            _mapper.Map(memberUpdateDto, user);
            _userRespository.Update(user);
            if (await _userRespository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user");
        }
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _userRespository.GetUserByUserName(User.GetUsername());

            var result = await _photservice.AddPhotoAsync(file);

            if(result.Error != null) return BadRequest(result.Error.Message);

            var Photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                publicId = result.PublicId
            };

            if (user.photos.Count==0)
            {
                Photo.IsMain = true;
            }
            user.photos.Add(Photo);

            if(await _userRespository.SaveAllAsync())
            {
                return CreatedAtRoute("GetUser",new {username = user.UserName},_mapper.Map<PhotoDto>(Photo));
                // return _mapper.Map<PhotoDto>(Photo);
            }
          

            return BadRequest("Problem adding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _userRespository.GetUserByUserName(User.GetUsername());

            var photo = user.photos.FirstOrDefault(x => x.Id == photoId);

            if (photo.IsMain) return BadRequest("This is already your main photo");

            var CurrentMain = user.photos.FirstOrDefault(x => x.IsMain);

            if(CurrentMain != null)  CurrentMain.IsMain = false;
            photo.IsMain = true;

            if(await _userRespository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to set Main Photo");
            
        }
        [HttpDelete("delete-photo/{photoId}")]

        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRespository.GetUserByUserName(User.GetUsername());
           
            var photo = user.photos.FirstOrDefault( x => x.Id == photoId);
            if (photo == null) return NotFound();
            if(photo.IsMain) return BadRequest("You cannot delete you main photo");
            if(photo.publicId !=null)
            {
               var result = await _photservice.DeletePhotoAsync(photo.publicId);
               if(result.Error != null) return BadRequest(result.Error.Message);

            }
            user.photos.Remove(photo);
            if(await _userRespository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to delete photo");

        }
    }
}