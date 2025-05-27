using Mapster;
using NotificationService.Contracts;
using RealEstate.BLL.Models;
using RealEstate.DAL.Entities;
using RealEstate.Presentation.DTOs.RealEstate;
using RealEstate.Presentation.DTOs.User;

namespace RealEstate.Presentation.Mapping
{
    public class MapsterConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<UserEntity, PartialUserDto>.NewConfig()
                .Map(dest => dest.FullName, src => $"{src.FirstName} {src.LastName}");

            TypeAdapterConfig<UserModel, PartialUserDto>.NewConfig()
                .Map(dest => dest.FullName, src => $"{src.FirstName} {src.LastName}");

            TypeAdapterConfig<RealEstateEntity, RealEstateDto>.NewConfig();

            TypeAdapterConfig<ReviewEntity, ReviewAddedEvent>.NewConfig()
                .Map(dest => dest.Email, src => src.Recipient.Email)
                .Map(dest => dest.FirstName, src => src.Recipient.FirstName)
                .Map(dest => dest.LastName, src => src.Recipient.LastName);

            TypeAdapterConfig<RealEstateEntity, RealEstateAddedEvent>.NewConfig()
                .Map(dest => dest.Email, src => src.Owner.Email)
                .Map(dest => dest.FirstName, src => src.Owner.FirstName)
                .Map(dest => dest.LastName, src => src.Owner.LastName);

            TypeAdapterConfig<RealEstateEntity, RealEstateDeletedEvent>.NewConfig()
                .Map(dest => dest.Email, src => src.Owner.Email)
                .Map(dest => dest.FirstName, src => src.Owner.FirstName)
                .Map(dest => dest.LastName, src => src.Owner.LastName);
        }
    }
}
