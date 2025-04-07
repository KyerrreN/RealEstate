﻿using Mapster;
using MapsterMapper;
using RealEstate.BLL.Exceptions;
using RealEstate.BLL.Interfaces;
using RealEstate.BLL.Models;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Interfaces;
using RealEstate.DAL.Models;
using RealEstate.DAL.RequestParameters;

namespace RealEstate.BLL.Services
{
    public class RealEstateService(IBaseRepository<RealEstateEntity> repository, IMapper mapper, IRealEstateRepository realEstateRepository) : GenericService<RealEstateEntity, RealEstateModel>(repository, mapper), IRealEstateService
    {
        private readonly IRealEstateRepository _realEstateRepository = realEstateRepository;

        public override async Task<RealEstateModel> CreateAsync(RealEstateModel model, CancellationToken ct)
        {
            model.EstateStatus = DAL.Enums.EstateStatus.Available;

            return await base.CreateAsync(model, ct);
        }

        public async Task<PagedEntityModel<RealEstateModel>> GetAllWithRequestParameters(RealEstateFilterParameters filters, CancellationToken ct)
        {
            CheckRealEstateRequestParameters(filters);

            var entities = await _realEstateRepository.GetAllWithRequestParameters(filters, ct);

            var modelList = entities.Adapt<PagedEntityModel<RealEstateModel>>();

            return modelList;
        }

        private static void CheckRealEstateRequestParameters(RealEstateFilterParameters filters)
        {
            if (filters.MinPrice < 0)
                throw new BadRequestException("Min price cannot be negative");

            if (filters.MinPrice > filters.MaxPrice)
                throw new BadRequestException("Max price must be greater than min price");
        }
    }
}
