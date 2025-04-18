﻿using RealEstate.BLL.Models;
using RealEstate.DAL.Entities;

namespace RealEstate.BLL.Interfaces
{
    public interface IBookingService : IGenericService<BookingEntity, BookingModel>
    {
        Task CloseDeal(CloseDealModel model, CancellationToken ct);
    }
}
