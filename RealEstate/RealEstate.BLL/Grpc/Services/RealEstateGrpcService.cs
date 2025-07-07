using Grpc.Core;
using RealEstate.DAL.Interfaces;
using RealEstate.Grpc;

namespace RealEstate.BLL.Grpc.Services
{
    public class RealEstateGrpcService(IRealEstateRepository repository) : RealEstateService.RealEstateServiceBase
    {
        public override async Task<GetByIdsResponse> GetByIds(GetByIdsRequest request, ServerCallContext context)
        {
            var ids = request.RealEstateIds
                .Select(id => Guid.Parse(id))
                .ToArray();

            var entities = await repository.GetByIdsAsync(ids, context.CancellationToken);

            var response = new GetByIdsResponse();
            response.RealEstates.AddRange(entities.Select(re => new RealEstateDto
            { 
                Id = re.Id.ToString(),
                Title = re.Title,
                Price = (double)re.Price,
            }));

            return response;
        }
    }
}
