using ChatService.BLL.Models;
using Mapster;
using MongoDB.Bson;
using СhatService.DAL.Documents;

namespace ChatService.API.MapsterConfig
{
    public static class MapsterConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<Message, MessageModel>.NewConfig()
                .Map(dest => dest.Id, src => src.Id.ToString());

            TypeAdapterConfig<MessageModel, Message>.NewConfig()
                .Map(dest => dest.Id, src => 
                    string.IsNullOrEmpty(src.Id) ? ObjectId.Empty : new ObjectId(src.Id));
        }
    }
}
