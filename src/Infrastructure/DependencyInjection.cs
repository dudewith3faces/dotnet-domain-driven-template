using System.Data;
using Application.Common.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // OPTIONS

        // SERVICES
        services.AddSingleton<ILoggerService, LoggerService>();

        // FAKE SERVICES

        // DATABASE
        services.AddSingleton<IDbConnection>(s => new MySqlConnection(configuration.GetConnectionString("DefaultConnection")));

        // OTHERS
        services.AddHttp();

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {

        return app;
    }

    private static IServiceCollection AddHttp(this IServiceCollection services)
    {
        services
                .AddHttpClient("BaseClient")
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
                {
                    AllowAutoRedirect = false,
                    UseDefaultCredentials = true,
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    SslProtocols = System.Security.Authentication.SslProtocols.Tls | System.Security.Authentication.SslProtocols.Tls11 | System.Security.Authentication.SslProtocols.Tls12,
                    ServerCertificateCustomValidationCallback = (message, cert, chain, policy) => true
                });

        return services;
    }
}