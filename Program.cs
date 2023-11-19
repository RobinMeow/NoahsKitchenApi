using api.Domain;
using api.Domain.Auth;
using api.Infrastructure;
using api.Infrastructure.Auth;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace api;

internal class Program
{
    static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<PersistenceSettings>(builder.Configuration.GetSection(nameof(PersistenceSettings)));

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(c =>
        {
            // Avoid having to type out the "Bearer " https://stackoverflow.com/a/64899768
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Noahs Kitchen", Version = "v1" });

            c.OperationFilter<SecurityRequirementsOperationFilter>();
            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
        });

        builder.Services.AddAuthentication().AddJwtBearer((options) =>
        {
            BearerConfig bearerConfig = builder.Configuration.GetSection(nameof(BearerConfig)).Get<BearerConfig>()!;

            options.TokenValidationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new IssuerSigningKeyFactory(builder.Configuration).Create(),
                ValidateIssuerSigningKey = true,
                ValidIssuer = bearerConfig.Issuer,
                ValidateIssuer = true,
                ValidAudiences = bearerConfig.Audiences,
                ValidateAudience = true,
                ValidateLifetime = true,
            };
        });

        builder.AddFrontEndOriginsCors();

        builder.Services.AddSingleton<DbContext, MongoDbContext>(); // Transient: instance per code request. Scoped: instance per HTTP request
        builder.Services.AddSingleton<IIssuerSigningKeyFactory, IssuerSigningKeyFactory>();
        builder.Services.AddSingleton<IPasswordHasher, AspPasswordHasher>();

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}

static class ProgrammExtensions
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
}
