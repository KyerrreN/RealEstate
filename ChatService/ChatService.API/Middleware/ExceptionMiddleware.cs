using ChatService.API.Constants;
using ChatService.Domain.Exceptions;
using ChatService.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace ChatService.API.Middleware
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (NotFoundException ex)
            {
                await HandleException(context, ex, StatusCodes.Status404NotFound);
            }
            catch (BadRequestException ex)
            {
                await HandleException(context, ex, StatusCodes.Status400BadRequest);
            }
            catch (ValidationException ex)
            {
                await HandleException(context, ex, StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex, StatusCodes.Status500InternalServerError);
            }
        }

        private async Task HandleException(HttpContext context, Exception ex, int statusCode)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = ApiConstants.JsonContentType;

            var errorModel = new ErrorModel
            {
                StatusCode = statusCode,
                Message = ex.Message
            };

            var jsonResponse = JsonSerializer.Serialize(errorModel);

            logger.LogError("{message} {newLine} {innerExceptionMessage}", ex.Message, Environment.NewLine, ex.InnerException?.Message);
            logger.LogError("Error query: {query}", context.Request.Path);
            logger.LogError("{stackTrace}", ex.StackTrace);

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
