using RealEstate.BLL.DI;
using RealEstate.Presentation.Configurations;

namespace RealEstate.Presentation.Extensions
{
    public static class ServiceExtensions
    {
        public static void RegisterBLLAndDbContext(this IServiceCollection services, IConfiguration config)
        {
            var dbContextConfig = new DbContextConfiguration
            {
                ConnectionString = config.GetSection("PostgreSQL").Get<DbContextConfiguration>()?.ConnectionString
            };

            if (dbContextConfig.ConnectionString is null)
            {
                throw new ArgumentException("Connection String is not configured");
            }
            else
            {
                services.RegisterBLL(dbContextConfig.ConnectionString);
            }
        }
    }
}
