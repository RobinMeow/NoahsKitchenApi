using System.Reflection;
using api.Domain;
using api.Domain.Auth;
using api.Infrastructure;
using api.Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace api;

public static class ProgrammExtensions
{
    public static IServiceCollection AddFrontEndOriginsCors(this WebApplicationBuilder builder)
    {
        IConfigurationSection corsSettingsSection = builder.Configuration.GetSection(nameof(CorsSettings));
        string[] allowedOrigins = corsSettingsSection.GetSection(nameof(CorsSettings.AllowedOrigins)).Get<string[]>()!;

        return builder.Services.AddCors((CorsOptions corsOptions) =>
        {
            corsOptions.AddDefaultPolicy((CorsPolicyBuilder corsPolicyBuilder) =>
            {
                corsPolicyBuilder.WithOrigins(allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
        });
    }

    public static IServiceCollection AddJwtAuthetication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IIssuerSigningKeyFactory, IssuerSigningKeyFactory>();
        services.AddSingleton<IPasswordHasher, AspPasswordHasher>();
        services.AddSingleton<IJwtFactory, JwtFactory>();

        AuthenticationBuilder authenticationBuilder = services.AddAuthentication();

        authenticationBuilder.AddJwtBearer((options) =>
        {
            BearerConfig bearerConfig = configuration.GetSection(nameof(BearerConfig)).Get<BearerConfig>()!;

            options.TokenValidationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new IssuerSigningKeyFactory(configuration).Create(),
                ValidateIssuerSigningKey = true,
                ValidIssuer = bearerConfig.Issuer,
                ValidateIssuer = true,
                ValidAudiences = bearerConfig.Audiences,
                ValidateAudience = true,
                ValidateLifetime = true,
            };
        });

        return services;
    }

    public static IServiceCollection AddSwaggerStuff(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SupportNonNullableReferenceTypes();

            c.CustomOperationIds((ApiDescription apiDescription) =>
            {
                return apiDescription.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;
            });

            // Avoid having to type out the "Bearer " https://stackoverflow.com/a/64899768
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Noahs Kitchen",
                Version = "v1",
                Contact = new OpenApiContact()
                {
                    Email = "robinmeow97@gmail.com",
                    Name = "Robin Daubenschütz",
                    Url = new Uri("https://robindaub.dev"),
                },
                License = new OpenApiLicense()
                {
                    Name = "MIT License",
                    // Url = 
                }
            });

            string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            string xmlDocumentationPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
            c.IncludeXmlComments(xmlDocumentationPath);

            c.OperationFilter<SecurityRequirementsOperationFilter>();

            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
        });

        return services;
    }
}
