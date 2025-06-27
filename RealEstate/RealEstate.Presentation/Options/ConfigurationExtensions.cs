namespace RealEstate.Presentation.Options
{
    public static class ConfigurationExtensions
    {
        public static T GetRequiredOptions<T>(this IConfiguration configuration, string sectionName) where T : class
        {
            var options = configuration.GetSection(sectionName).Get<T>();

            if (options is null)
                throw new InvalidOperationException($"Failed to bind {typeof(T).Name}, check for missing or invalid configuration values");

            return options;
        }
    }
}
