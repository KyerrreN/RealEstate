using ChatService.BLL.Interface;
using ChatService.BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChatService.API
{
    public static class MessageEndpoints
    {
        public static void MapMessageEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/messages", async (
                [FromBody] CreateMessageModel model,
                IMessageService service,
                CancellationToken ct) =>
            {
                var result = await service.AddMessageAsync(model, ct);

                return Results.Ok(result);
            });

            app.MapGet("api/messages/{userId}/realestate/{realEstateId:guid}", async (
                IMessageService service,
                string userId,
                Guid realEstateId,
                CancellationToken ct) =>
            {
                var result = await service.GetAllAsync(realEstateId, userId, ct);

                return Results.Ok(result);
            });

            app.MapGet("api/dialogs/{userId}", async (
                IMessageService service,
                string userId,
                CancellationToken ct) =>
            {
                var result = await service.GetUserDialogsAsync(userId, ct);

                return Results.Ok(result);
            });
        }
    }
}
