using System.Security.Claims;

namespace ChatService.API.Filters
{
    public class RequireAndSetUserIdFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Results.Unauthorized();
            }

            context.HttpContext.Items["UserId"] = userId;

            return await next(context);
        }
    }
}
