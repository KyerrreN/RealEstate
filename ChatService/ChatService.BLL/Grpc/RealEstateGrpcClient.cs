using RealEstate.Grpc;

namespace ChatService.BLL.Grpc
{
    public class RealEstateGrpcClient(RealEstateService.RealEstateServiceClient client)
    {
        public async Task<IList<RealEstateDto>> GetByIdsAsync(IEnumerable<string> ids, CancellationToken ct)
        {
            var request = new GetByIdsRequest();
            request.RealEstateIds.AddRange(ids);

            var response = await client.GetByIdsAsync(request, cancellationToken: ct);
            return response.RealEstates;
        }
    }
}
