using NotificationService.Contracts;

namespace NotificationService.BLL.Interfaces
{
    public interface IEmailService<T> where T : BaseEvent
    {
        Task Send(T data, string path, string subject, CancellationToken ct);
    }
}
