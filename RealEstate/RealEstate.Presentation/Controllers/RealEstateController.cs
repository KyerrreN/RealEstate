using Mapster;
using Microsoft.AspNetCore.Mvc;
using RealEstate.BLL.Interfaces;
using RealEstate.BLL.Models;
using RealEstate.DAL.RequestParameters;
using RealEstate.Presentation.Constants;
using RealEstate.Presentation.DTOs;
using RealEstate.Presentation.DTOs.RealEstate;

namespace RealEstate.Presentation.Controllers
{
    [Route(ApiRoutes.RealEstateEndpoint)]
    [ApiController]
    public class RealEstateController(IRealEstateService realEstateService) : ControllerBase
    {
        private readonly IRealEstateService _realEstateService = realEstateService;

        [HttpGet]
        public async Task<PagedEntityDto<RealEstateDto>> GetAll([FromQuery] RealEstateFilterParameters filters,CancellationToken ct)
        {
            var realEstateModels = await _realEstateService.GetAllWithRequestParameters(filters, ct);

            var realEstateDtos = realEstateModels.Adapt<PagedEntityDto<RealEstateDto>>();

            return realEstateDtos;
        }

        [HttpGet("{id:guid}")]
        public async Task<RealEstateDto> GetById(Guid id, CancellationToken ct)
        {
            var realEstateModel = await _realEstateService.GetByIdAsync(id, ct);

            var realEstateDto = realEstateModel.Adapt<RealEstateDto>();

            return realEstateDto;
        }

        [HttpPost]
        public async Task<RealEstateDto> Create([FromBody] CreateRealEstateDto realEstateForCreationDto, CancellationToken ct)
        {
            var realEstateModel = realEstateForCreationDto.Adapt<RealEstateModel>();

            var createdRealEstateModel = await _realEstateService.CreateAsync(realEstateModel, ct);

            var createdRealEstateDto = createdRealEstateModel.Adapt<RealEstateDto>();

            return createdRealEstateDto;
        }

        [HttpDelete("{id:guid}")]
        public async Task<NoContentResult> Delete(Guid id, CancellationToken ct)
        {
            await _realEstateService.DeleteAsync(id, ct);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public async Task<RealEstateDto> Update(Guid id, [FromBody] UpdateRealEstateDto dto, CancellationToken ct)
        {
            var realEstateModel = dto.Adapt<RealEstateModel>();

            var updatedRealEstateModel = await _realEstateService.UpdateAsync(id, realEstateModel, ct);

            var resultDto = updatedRealEstateModel.Adapt<RealEstateDto>();

            return resultDto;
        }
    }
}
