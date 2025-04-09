using Mapster;
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

            TypeAdapterConfig<RealEstateEntity, RealEstateDto>.NewConfig();
        }
    }
}
