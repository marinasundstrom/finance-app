namespace Accountant;

public static class ConfigurationExtensions
{
    public static string GetConnectionString2(this IConfiguration configuration, string name, string database)
    {
        var connectionString = configuration.GetConnectionString(name);
        return $"{connectionString};Database={database}";
    }
}