using ChatService.API.Constants;
using ChatService.API.DTO;
using ChatService.API.Filters;
using ChatService.BLL.Interface;
using ChatService.BLL.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;

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
                var model = dto.Adapt<CreateMessageModel>(); 
                
                var result = await service.AddMessageAsync(model, context.Items["UserId"] as string, ct);

                return Results.Ok(result);
            })
                .AddEndpointFilter<RequireUserIdFilter>()
                .RequireAuthorization();

            app.MapGet(ApiConstants.RouteGetMessages, async (
                IMessageService service,
                HttpContext context,
                Guid realEstateId,
                CancellationToken ct) =>
            {
                var result = await service.GetAllAsync(realEstateId, context.Items["UserId"]! as string, ct);

                return Results.Ok(result);
            })
                .AddEndpointFilter<RequireUserIdFilter>()
                .RequireAuthorization();

            app.MapGet(ApiConstants.RouteGetUserDialogs, async (
                IMessageService service,
                HttpContext context,
                CancellationToken ct) =>
            {
                var result = await service.GetUserDialogsAsync(context.Items["UserId"] as string, ct);

                return Results.Ok(result);
            })
                .AddEndpointFilter<RequireUserIdFilter>()
                .RequireAuthorization();
        }
    }
}
