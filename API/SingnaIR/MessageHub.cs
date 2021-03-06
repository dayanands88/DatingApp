using System;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace API.SingnaIR
{
    public class MessageHub : Hub
    {
        public IMessageRepository _messageRepository { get; set; }
        public IMapper _Mapper { get; set; }
        public IUserRepository _userRepository { get; set; }
        public MessageHub(IMessageRepository messageRepository, IMapper mapper,
        

        IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _Mapper = mapper;
            _messageRepository = messageRepository;

        }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var otherUser = httpContext.Request.Query["user"].ToString();
        var groupName = GetGroupName(Context.User.GetUsername(), otherUser);

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        var message = await _messageRepository.
            GetMessageThred(Context.User.GetUsername(), otherUser);
        await Clients.Group(groupName).SendAsync("ReceiveMessageThread", message);

    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(CreateMessageDto createMessageDto)
    {
        var username = Context.User.GetUsername();
        //    var username = _userRepository.GetUserByIdAsync(Convert.ToInt32(usernameId));
        if (username == createMessageDto.RecipientUsername.ToLower())
            throw new HubException("You cannot send messgae to yourself");
        var sender = await _userRepository.GetUserByUserName(username);
        var recipient = await _userRepository.GetUserByUserName(createMessageDto.RecipientUsername);
        if (recipient == null) throw new HubException("Not found user");
        var message = new Message
        {
            Sender = sender,
            Recipient = recipient,
            SendingUsername = sender.UserName,
            RecipientUsername = recipient.UserName,
            Content = createMessageDto.Content
        };

        _messageRepository.AddMessage(message);

        if (await _messageRepository.SaveAllAsync())
        {
            var group = GetGroupName(sender.UserName, recipient.UserName);
            await Clients.Group(group).SendAsync("NewMessage", _Mapper.Map<MessageDto>(message));
        }
    
    }

    private string GetGroupName(string caller, string other)
    {
        var stringCompare = string.CompareOrdinal(caller, other) < 0;
        return stringCompare ? $"{caller}--{other}" : $"{other}--{caller}";
    }
}
}