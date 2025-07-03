using ChatService.BLL.Models;
using ChatService.Domain.Models;

namespace ChatService.BLL.Interface
{
    public interface IMessageService
    {
        Task<MessageModel> AddMessageAsync(CreateMessageModel model, string userId, CancellationToken ct);
        Task<IEnumerable<MessageModel>> GetAllAsync(Guid realEstateId, string userId, CancellationToken ct);
        Task<IEnumerable<DialogPreviewModel>> GetUserDialogsAsync(string userId, CancellationToken ct);
    }
}
