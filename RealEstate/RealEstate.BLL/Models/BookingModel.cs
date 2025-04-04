﻿using RealEstate.DAL.Enums;

namespace RealEstate.BLL.Models
{
    public class BookingModel : BaseModel
    {
        public Guid RealEstateId { get; set; }
        public string Proposal { get; set; } = null!;
        public EstateAction EstateAction { get; set; }
    }
}
