using System.Reflection;
using Microsoft.OpenApi.Models;
using Presentation.Options;

namespace Presentation.Extensions;
public static class SwaggerExtension
{
    public static IServiceCollection AddSwagger(this IServiceCollection service, IConfiguration configuration)
    {
        SwaggerOption swaggerOption = new();

        configuration.GetSection(nameof(SwaggerOption)).Bind(swaggerOption);

        if (swaggerOption.ShowSwagger)
            service
               .AddSwaggerGen(c =>
               {
                   string[]? versions = swaggerOption.Versions;
                   if (versions != null && versions.Length > 0)
                       foreach (var version in versions) c.SwaggerDoc(version, new OpenApiInfo { Title = swaggerOption.Title, Version = version });

                   c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
               });

        return service;
    }

    public static IApplicationBuilder UseSwaggerMiddleware(this IApplicationBuilder app, IConfiguration configuration)
    {
        SwaggerOption swaggerOption = new();
        configuration.GetSection(nameof(SwaggerOption)).Bind(swaggerOption);

        if (swaggerOption.ShowSwagger)
            app.UseSwagger()
               .UseSwaggerUI(c =>
                {
                    c.DocumentTitle = swaggerOption.Title;
                    c.SwaggerEndpoint(swaggerOption.Endpoint, swaggerOption.Title);
                });

        return app;
    }
}
