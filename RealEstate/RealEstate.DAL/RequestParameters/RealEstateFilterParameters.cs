using RealEstate.DAL.Enums;

namespace RealEstate.DAL.RequestParameters
{
    public class RealEstateFilterParameters : PagingParameters
    {
        public List<EstateType>? EstateType { get; set; }
        public List<EstateStatus>? EstateStatus { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public Guid? OwnerId { get; set; }
        public string? City { get; set; }

        public string? OrderBy { get; set; } = "CreatedAt desc";
    }
}
