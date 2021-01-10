using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        public MessagesController(IUserRepository userRepository, IMessageRepository messageRepository, IMapper mapper)
        {
            _mapper = mapper;
            _messageRepository = messageRepository;
            _userRepository = userRepository;

        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username = User.GetUsername();
        //    var username = _userRepository.GetUserByIdAsync(Convert.ToInt32(usernameId));
            if (username == createMessageDto.RecipientUsername.ToLower())
                return BadRequest("You cannot send messgae to yourself");
            var sender = await _userRepository.GetUserByUserName(username);
            var recipient = await _userRepository.GetUserByUserName(createMessageDto.RecipientUsername);
            if (recipient == null) return NotFound();
            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SendingUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content
            };

            _messageRepository.AddMessage(message);

            if(await _messageRepository.SaveAllAsync()) return Ok(_mapper.Map<MessageDto>(message));

            return BadRequest("Failed to send message");
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery]
             MessageParams messageParams) 
        {
                messageParams.Username = User.GetUsername();
                // var username1 = _userRepository.GetUserByIdAsync(Convert.ToInt32(User.GetUsername()));
                // var user = await _userRepository.GetUserByIdAsync(Convert.ToInt32(User.GetUsername()));
                //  userParams.CurrentUsername = user?.UserName;
                //  messageParams.Username = username.UserName;
                // messageParams.Username = user.UserName;
                var message = await _messageRepository.GetMessagesForUser(messageParams);
                Response.AddPaginationHeader(message.CurrentPage, message.PageSize,
                    message.TotalCount,message.TotalPages); 
                return message;               
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetMessageThread(string username)
        {
            var currentUsername = User.GetUsername();
            // var user = await _userRepository.GetUserByIdAsync(User.GetUsername());
            // var currentUsername = user.UserName;
            return Ok(await _messageRepository.GetMessageThred(currentUsername,username));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            //  var user = await _userRepository.GetUserByIdAsync(Convert.ToInt32(User.GetUsername()));
            var UserName = User.GetUsername();
             var message = await _messageRepository.GetMessage(id);
             if (message.Sender.UserName != UserName && message.Recipient.UserName != UserName)
                return Unauthorized();
             if (message.Sender.UserName == UserName) message.SenderDeleted = true;
             if (message.Recipient.UserName ==  UserName) message.RecipientDeleted = true;
             if (message.SenderDeleted && message.RecipientDeleted)
                _messageRepository.DeleteMessage(message);
             if (await _messageRepository.SaveAllAsync()) return Ok();
             return BadRequest("Problem deleting the message");
        }
    }
}