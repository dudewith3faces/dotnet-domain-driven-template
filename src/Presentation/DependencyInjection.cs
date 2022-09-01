using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Presentation.Extensions;
using Presentation.Filters;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        services.AddHealthChecks();
        services.AddHttpContextAccessor();

        services.AddSwagger(configuration);
        services.AddControllers(x =>
                {
                    x.Filters.Add<ValidationFilter>();
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
                .AddFluentValidation(options =>
                {
                    options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                });

        services.AddApiVersioning(options =>
                    {
                        options.AssumeDefaultVersionWhenUnspecified = true;
                        options.DefaultApiVersion = ApiVersion.Default;
                        options.ApiVersionReader = new HeaderApiVersionReader("api-version");
                        options.ReportApiVersions = true;
                    });

        return services;
    }

    public static IApplicationBuilder UsePresentation(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseHealthChecks("/health");

        app.UseSwaggerMiddleware(configuration)
          .UseForwardedHeaders(new ForwardedHeadersOptions
          {
              ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
          })
          .UseCors()
          .UseCustomExceptionHandler()
          .UseRouting()
          .UseAuthorization()
          .UseEndpoints(endpoints =>
          {
              endpoints.MapControllers();
          });

        return app;
    }
}