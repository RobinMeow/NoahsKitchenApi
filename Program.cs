using api.Domain;
using api.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

        builder.Services.AddSwaggerGen();

        builder.AddFrontEndOriginsCors();

        // Singleton: instance per 'deploy' (per application lifetime)
        // Scoped: instance per HTTP request
        // Transient: instance per code request.
        builder.Services.AddScoped<DbContext, MongoDbContext>(); // Apperently its best practise to have it a singleton. I dont see a reason to leave a db connection open for ever. So I stick to scoped. Retrieving a connection from to pool and returning it once per http request seems more reasonable. (I will probably end up changing this later, as soon as I realize, that EFCore uses scoped pool connections internally)

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors();
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
