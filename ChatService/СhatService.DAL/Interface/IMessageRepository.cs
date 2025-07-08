using ChatService.Domain.Models;
using СhatService.DAL.Documents;

namespace СhatService.DAL.Interface
{
    public interface IMessageRepository
    {
        Task<Message> AddAsync(Message message, CancellationToken ct);
        Task<IEnumerable<Message>> GetAllAsync(Guid realEstateId, string userId, CancellationToken ct);
        Task<List<DialogPreviewModel>> GetUserDialogsAsync(string userId, CancellationToken ct);
    }
}
