using Mapster;
using Microsoft.AspNetCore.Mvc;
using RealEstate.BLL.Interfaces;
using RealEstate.BLL.Models;
using RealEstate.Presentation.Constants;
using RealEstate.Presentation.DTOs;

namespace RealEstate.Presentation.Controllers
{
    [Route(ApiRoutes.RealEstateEndpoint)]
    [ApiController]
    public class RealEstateController(IRealEstateService realEstateService) : ControllerBase
    {
        private readonly IRealEstateService _realEstateService = realEstateService;

        [HttpGet]
        public async Task<List<RealEstateDto>> GetAll(CancellationToken ct)
        {
            var realEstateModels = await _realEstateService.GetAllAsync(ct);

            var realEstateDtos = realEstateModels.Adapt<List<RealEstateDto>>();

            return realEstateDtos;
        }

        [HttpGet("{reId:guid}")]
        public async Task<RealEstateDto> GetOne(Guid reId, CancellationToken ct)
        {
            var realEstateModel = await _realEstateService.GetByIdAsync(reId, ct);

            var realEstateDto = realEstateModel.Adapt<RealEstateDto>();

            return realEstateDto;
        }

        [HttpPost]
        public async Task<RealEstateDto> CreatePosting([FromBody] RealEstateForCreationDto realEstateForCreationDto, CancellationToken ct)
        {
            var realEstateModel = realEstateForCreationDto.Adapt<RealEstateModel>();

            var createdRealEstateModel = await _realEstateService.CreateAsync(realEstateModel, ct);

            var createdRealEstateDto = createdRealEstateModel.Adapt<RealEstateDto>();

            return createdRealEstateDto;
        }
    }
}
