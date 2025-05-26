using NotificationService.Contracts;

namespace NotificationService.BLL.Interfaces
{
    public interface IEmailService
    {
        Task SendUserRegisterAsync(UserRegisteredEvent userMetadata, CancellationToken ct);
        Task SendRealEstateAddedAsync(RealEstateAddedEvent realEstateMetadata, CancellationToken ct);
    }
}
