using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.BLL.Interfaces;
using RealEstate.Domain.QueryParameters;
using RealEstate.Presentation.Constants;
using RealEstate.Presentation.DTOs;
using RealEstate.Presentation.DTOs.History;

namespace RealEstate.Presentation.Controllers
{
    [Route(ApiRoutes.HistoryEndpoint)]
    [ApiController]
    [Authorize]
    public class HistoryController(IHistoryService _historyService) : ControllerBase
    {
        [HttpGet]
        public async Task<PagedEntityDto<HistoryDto>> GetAll([FromQuery] PagingParameters paging, Guid userId, CancellationToken ct)
        {
            var historyModels = await _historyService.GetAllByOwnerIdAsync(paging, userId, ct);

            var historyDtos = historyModels.Adapt<PagedEntityDto<HistoryDto>>();

            return historyDtos;
        }

        [HttpGet("{historyId:guid}")]
        public async Task<HistoryDto> GetOne(Guid historyId, Guid userId, CancellationToken ct)
        {
            var historyModel = await _historyService.GetOneByOwnerIdAsync(historyId, userId, ct);

            var historyDto = historyModel.Adapt<HistoryDto>();

            return historyDto;
        }

        [HttpDelete("{historyId:guid}")]
        public async Task<NoContentResult> Delete(Guid historyId, Guid userId, CancellationToken ct)
        {
            await _historyService.DeleteFromHistoryAsync(historyId, userId, ct);

            return NoContent();
        }
    }
}
