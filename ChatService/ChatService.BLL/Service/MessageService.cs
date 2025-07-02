using ChatService.BLL.Interface;
using ChatService.BLL.Models;
using ChatService.Domain.Models;
using Mapster;
using СhatService.DAL.Documents;
using СhatService.DAL.Interface;

namespace ChatService.BLL.Service
{
    public class MessageService(IMessageRepository repository) : IMessageService
    {
        public async Task<MessageModel> AddMessageAsync(CreateMessageModel model, CancellationToken ct)
        {
            var message = model.Adapt<Message>();
            message.SentAt = DateTime.UtcNow;

            var result = await repository.AddAsync(message, ct);

            return result.Adapt<MessageModel>();
        }

        public async Task<IEnumerable<MessageModel>> GetAllAsync(Guid realEstateId, string userId, CancellationToken ct)
        {
            var message = await repository.GetAllAsync(realEstateId, userId, ct);

            var messageModel = message.Adapt<IEnumerable<MessageModel>>();

            return messageModel.ToList();
        }

        public async Task<IEnumerable<DialogPreviewModel>> GetUserDialogsAsync(string userId, CancellationToken ct)
        {
            var dialogs = await repository.GetUserDialogsAsync(userId, ct);

            var dialogsModel = dialogs.Adapt<IEnumerable<DialogPreviewModel>>();

            return dialogsModel.ToList();
        }
    }
}
