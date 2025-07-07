using ChatService.BLL.Grpc;
using ChatService.BLL.Interface;
using ChatService.BLL.Models;
using ChatService.Domain.Models;
using Mapster;
using СhatService.DAL.Documents;
using СhatService.DAL.Interface;

namespace ChatService.BLL.Service
{
    public class MessageService(
        IMessageRepository repository,
        RealEstateGrpcClient grpcClient) : IMessageService
    {
        public async Task<MessageModel> AddMessageAsync(CreateMessageModel model, string userId, CancellationToken ct)
        {
            var message = model.Adapt<Message>();
            message.SenderId = userId;
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
            var dialogList = dialogs.ToList();

            var realEstateIds = dialogList
                .Select(d => d.RealEstateId.ToString())
                .Distinct()
                .ToList();

            var grpcRealEstates = await grpcClient.GetByIdsAsync(realEstateIds, ct);

            var realEstateDict = grpcRealEstates.ToDictionary(
                re => Guid.Parse(re.Id),
                re => new RealEstateModel
                {
                    Title = re.Title,
                    Price = re.Price,
                });

            var dialogsModel = dialogList
                .Select(dialog =>
                {
                    var model = dialog.Adapt<DialogPreviewModel>();

                    if (realEstateDict.TryGetValue(dialog.RealEstateId, out var realEstate))
                    {
                        model.RealEstate = realEstate;
                    }

                    return model;
                });

            return dialogsModel.ToList();
        }
    }
}
