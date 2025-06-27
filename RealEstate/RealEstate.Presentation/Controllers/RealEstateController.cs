using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.BLL.Interfaces;
using RealEstate.BLL.Models;
using RealEstate.Domain.QueryParameters;
using RealEstate.Presentation.Constants;
using RealEstate.Presentation.DTOs;
using RealEstate.Presentation.DTOs.RealEstate;

namespace RealEstate.Presentation.Controllers
{
    [Route(ApiRoutes.RealEstateEndpoint)]
    [ApiController]
    [Authorize]
    public class RealEstateController(IRealEstateService _realEstateService,
        IValidator<CreateRealEstateDto> _createRealEstateValidator,
        IValidator<UpdateRealEstateDto> _updateRealEstateValidator) 
        : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<PagedEntityDto<RealEstateDto>> GetAll([FromQuery] RealEstateFilterParameters filters, [FromQuery] SortingParameters sorting, CancellationToken ct)
        {
            var realEstateModels = await _realEstateService.GetAllWithRequestParameters(filters, sorting, ct);

            var realEstateDtos = realEstateModels.Adapt<PagedEntityDto<RealEstateDto>>();

            return realEstateDtos;
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<RealEstateDto> GetById(Guid id, CancellationToken ct)
        {
            var realEstateModel = await _realEstateService.GetByIdAsync(id, ct);

            var realEstateDto = realEstateModel.Adapt<RealEstateDto>();

            return realEstateDto;
        }

        [HttpPost]
        public async Task<RealEstateDto> Create([FromBody] CreateRealEstateDto realEstateForCreationDto, CancellationToken ct)
        {
            await _createRealEstateValidator.ValidateAndThrowAsync(realEstateForCreationDto, ct);

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
            await _updateRealEstateValidator.ValidateAndThrowAsync(dto, ct);

            var realEstateModel = dto.Adapt<RealEstateModel>();

            var updatedRealEstateModel = await _realEstateService.UpdateAsync(id, realEstateModel, ct);

            var resultDto = updatedRealEstateModel.Adapt<RealEstateDto>();

            return resultDto;
        }
    }
}
