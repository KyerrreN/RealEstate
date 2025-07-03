using ChatService.API.Constants;
using ChatService.API.DTO;
using ChatService.BLL.Interface;
using ChatService.BLL.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Security.Claims;

namespace ChatService.API
{
    public static class MessageEndpoints
    {
        public static void MapMessageEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost(ApiConstants.RouteSaveMessage, async (
                [FromBody] CreateMessageDto dto,
                HttpContext context,
                IMessageService service,
                CancellationToken ct) =>
            {
                var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                Console.WriteLine(userId);

                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }

                var model = dto.Adapt<CreateMessageModel>(); 
                
                var result = await service.AddMessageAsync(model, userId, ct);

                return Results.Ok(result);
            }).RequireAuthorization();

            app.MapGet(ApiConstants.RouteGetMessages, async (
                IMessageService service,
                string userId,
                Guid realEstateId,
                CancellationToken ct) =>
            {
                var result = await service.GetAllAsync(realEstateId, userId, ct);

                return Results.Ok(result);
            }).RequireAuthorization();

            app.MapGet(ApiConstants.RouteGetUserDialogs, async (
                IMessageService service,
                string userId,
                CancellationToken ct) =>
            {
                var result = await service.GetUserDialogsAsync(userId, ct);

                return Results.Ok(result);
            }).RequireAuthorization();
        }
    }
}
