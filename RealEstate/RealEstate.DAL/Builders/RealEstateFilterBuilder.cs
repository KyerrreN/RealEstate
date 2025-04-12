using RealEstate.DAL.Entities;
using RealEstate.DAL.Enums;

namespace RealEstate.DAL.Builders
{
    public interface IRealEstateFilterBuilder
    {
        RealEstateFilterBuilder SetEstateStatus(List<EstateStatus>? estateStatuses);
        RealEstateFilterBuilder SetEstateType();
        RealEstateFilterBuilder SetOwner();
        RealEstateFilterBuilder SetCity(string? city);
        RealEstateFilterBuilder SetPrice();
        IQueryable<RealEstateEntity> Build(IQueryable<RealEstateEntity> realEstateQuery, CancellationToken ct);
    }

    public class RealEstateFilterBuilder : IRealEstateFilterBuilder
    {
        private string? _city;
        private List<EstateStatus>? _estateStatuses;
        //private IQueryable<EstateType> _estateType;
       
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

        public RealEstateFilterBuilder SetEstateType()
        {
            throw new NotImplementedException();
        }

        public RealEstateFilterBuilder SetOwner()
        {
            throw new NotImplementedException();
        }

        public RealEstateFilterBuilder SetPrice()
        {
            throw new NotImplementedException();
        }

        public IQueryable<RealEstateEntity> Build(IQueryable<RealEstateEntity> realEstateQuery, CancellationToken ct)
        {
            return realEstateQuery.Where(re =>
                (!string.IsNullOrEmpty(_city) || re.City == _city) &&
                (_estateStatuses != null || _estateStatuses.Count == 0 || _estateStatuses.Contains(re.EstateStatus))
            );  
        }
    }
}
