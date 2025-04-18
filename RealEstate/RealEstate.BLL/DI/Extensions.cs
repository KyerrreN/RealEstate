﻿using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstate.BLL.Interfaces;
using RealEstate.BLL.Services;
using RealEstate.DAL.DI;

namespace RealEstate.BLL.DI
{
    public static class Extensions
    {
        public static void RegisterBLL(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterDataAccess(configuration);
            services.AddMapster();

            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IHistoryService, HistoryService>();
            services.AddScoped<IRealEstateService, RealEstateService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}
