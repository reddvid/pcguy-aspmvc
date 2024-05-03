using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PCGuy.DataAccess.Data;

public static class ApplicationDbContextExtensions
{
    public static IServiceCollection AddApplicationContext(
        this IServiceCollection services,
        string? connectionString = null)
    {
        if (connectionString is null)
        {
            SqlConnectionStringBuilder builder = new()
            {
                DataSource = ".",
                InitialCatalog = "PCGuy",
                TrustServerCertificate = true,
                MultipleActiveResultSets = true,
                ConnectTimeout = 30,
                IntegratedSecurity = true
            };
            connectionString = builder.ConnectionString;
        }

        services.AddDbContext<ApplicationDbContext>(
            options =>
            {
                options.UseSqlServer(connectionString);
                options.LogTo(Console.WriteLine,
                    new[]
                    {
                        Microsoft.EntityFrameworkCore
                            .Diagnostics.RelationalEventId
                            .CommandExecuting
                    });
            },
            contextLifetime: ServiceLifetime.Transient,
            optionsLifetime: ServiceLifetime.Transient);
        return services;
    }
}