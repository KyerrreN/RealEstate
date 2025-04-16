using RealEstate.BLL.DI;
using RealEstate.Presentation.Mapping;
using RealEstate.Presentation.Middleware;

namespace RealEstate.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            MapsterConfig.RegisterMappings();

            builder.Services.RegisterBLL(builder.Configuration);

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
