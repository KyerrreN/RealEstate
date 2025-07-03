using ChatService.API.Constants;
using ChatService.BLL.Interface;
using ChatService.BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChatService.API
{
    public static class MessageEndpoints
    {
        public static void MapMessageEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost(ApiConstants.RouteSaveMessage, async (
                [FromBody] CreateMessageModel model,
                IMessageService service,
                CancellationToken ct) =>
            {
                var result = await service.AddMessageAsync(model, ct);

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
