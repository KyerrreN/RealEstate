using RealEstate.DAL.Entities;
using RealEstate.DAL.Enums;

namespace RealEstate.DAL.Builders
{
    public interface IRealEstateFilterBuilder
    {
        RealEstateFilterBuilder SetEstateStatus(List<EstateStatus>? estateStatuses);
        RealEstateFilterBuilder SetEstateType(List<EstateType>? estateTypes);
        RealEstateFilterBuilder SetOwner(Guid? ownerId);
        RealEstateFilterBuilder SetCity(string? city);
        RealEstateFilterBuilder SetPrice(decimal? minPrice, decimal? maxPrice);
        IQueryable<RealEstateEntity> Build(IQueryable<RealEstateEntity> realEstateQuery, CancellationToken ct);
    }

    public class RealEstateFilterBuilder : IRealEstateFilterBuilder
    {
        private string? _city;
        private List<EstateStatus>? _estateStatuses;
        private List<EstateType>? _estateTypes;
        private Guid? _ownerId;
        private decimal? _minPrice;
        private decimal? _maxPrice;

        private bool _validPriceRange = false;

        public RealEstateFilterBuilder SetCity(string? city)
        {
            if (string.IsNullOrEmpty(city))
                return this;

            _city = city;

            return this;
        }

        public RealEstateFilterBuilder SetEstateStatus(List<EstateStatus>? estateStatuses)
        {
            if (estateStatuses is null || estateStatuses.Count == 0)
                return this;

            _estateStatuses = estateStatuses;

            return this;
        }

        public RealEstateFilterBuilder SetEstateType(List<EstateType>? estateTypes)
        {
            if (estateTypes is null || estateTypes.Count == 0)
                return this;

            _estateTypes = estateTypes;

            return this;
        }

        public RealEstateFilterBuilder SetOwner(Guid? ownerId)
        {
            if (!ownerId.HasValue || ownerId.Value == Guid.Empty)
                return this;

            _ownerId = ownerId;

            return this;
        }

        public RealEstateFilterBuilder SetPrice(decimal? minPrice, decimal? maxPrice)
        {
            if (minPrice.HasValue && minPrice > 0)
            {
                _minPrice = minPrice;
            }

            if (maxPrice.HasValue && maxPrice > 0)
            {
                _maxPrice = maxPrice;
            }

            if (minPrice.HasValue && maxPrice.HasValue && minPrice <= maxPrice)
            {
                _validPriceRange = true;
            }

            return this;
        }

        public IQueryable<RealEstateEntity> Build(IQueryable<RealEstateEntity> realEstateQuery, CancellationToken ct)
        {
            return realEstateQuery.Where(re =>
                (string.IsNullOrEmpty(_city) || re.City.ToLower().Trim() == _city.ToLower().Trim()) &&
                (_estateStatuses == null || _estateStatuses.Contains(re.EstateStatus)) &&
                (_ownerId == null || re.OwnerId == _ownerId) &&
                (_estateTypes == null || _estateTypes.Contains(re.EstateType)) &&
                (_validPriceRange == false || _minPrice == null || _maxPrice != null || (re.Price >= _minPrice && re.Price <= _maxPrice)) &&
                (_minPrice == null || re.Price >= _minPrice) &&
                (_maxPrice == null || re.Price <= _maxPrice)
            );
        }
    }
}
