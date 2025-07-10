using ChatService.API.DTO;
using ChatService.BLL.Interface;
using ChatService.BLL.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChatService.API.Hubs
{
    public class ChatHub(IMessageService service) : Hub
    {
        public async Task JoinDialog(string dialogId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, dialogId);
        }

        [Authorize]
        public async Task SendMessage(CreateMessageDto messageDto)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine("USER ID: " + userId);
            if (string.IsNullOrEmpty(userId))
            {
                throw new HubException("User is not authenticated.");
            }

            var messageModel = messageDto.Adapt<CreateMessageModel>();
            messageModel.SenderId = userId;

            var message = await service.AddMessageAsync(messageModel, userId, CancellationToken.None);

            await Clients.Group(messageDto.RealEstateId.ToString()).SendAsync("ReceiveMessage", message);
        }
    }
}
