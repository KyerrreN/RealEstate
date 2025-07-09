using ChatService.API.DTO;
using ChatService.BLL.Interface;
using ChatService.BLL.Models;
using Mapster;
using Microsoft.AspNetCore.SignalR;

namespace ChatService.API.Hubs
{
    public class ChatHub(IMessageService service) : Hub
    {
        public async Task JoinDialog(string dialogId, CancellationToken ct)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, dialogId, ct);
        }

        public async Task SendMessage(CreateMessageDto messageDto, CancellationToken ct)
        {
            var userId = Context.User?.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
            {
                throw new HubException("User is not authenticated.");
            }

            var messageModel = messageDto.Adapt<CreateMessageModel>();
            messageModel.SenderId = userId;

            var message = await service.AddMessageAsync(messageModel, userId, ct);

            await Clients.Group(messageDto.RealEstateId.ToString()).SendAsync("RecieveMessage", message, ct);
        }
    }
}
