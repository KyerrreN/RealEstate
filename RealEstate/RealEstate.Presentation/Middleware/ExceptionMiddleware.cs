using Microsoft.AspNetCore.Diagnostics;
using RealEstate.BLL.Exceptions;
using System.Net;
using System.Text.Json;

namespace RealEstate.Presentation.Middleware
{
    public class ExceptionMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                if (contextFeature != null)
                {
                    context.Response.StatusCode = contextFeature.Error switch
                    {
                        NotFoundException => StatusCodes.Status404NotFound,
                        BadRequestException => StatusCodes.Status400BadRequest,
                        _ => StatusCodes.Status500InternalServerError
                    };
                }

                var response = new
                {
                    contextFeature?.Error.Message,
                };

                var jsonResponse = JsonSerializer.Serialize(response);
                await context.Response.WriteAsJsonAsync(jsonResponse);
            }
        }
    }
}
