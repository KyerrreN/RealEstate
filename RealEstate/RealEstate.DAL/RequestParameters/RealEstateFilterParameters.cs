using RealEstate.DAL.Enums;

namespace RealEstate.DAL.RequestParameters
{
    public class RealEstateFilterParameters : PagingParameters
    {
        public EstateType? EstateType { get; set; }
        public List<EstateStatus>? EstateStatus { get; set; }
        public decimal? MinPrice { get; set; } = 0;
        public decimal? MaxPrice { get; set; } = decimal.MaxValue;
        public Guid? OwnerId { get; set; }
        public string? City { get; set; }

        public string? OrderBy { get; set; } = "CreatedAt desc";
    }
}
