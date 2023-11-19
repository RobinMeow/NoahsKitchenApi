using api.Domain;
using api.Domain.Auth;
using api.Infrastructure;
using api.Infrastructure.Auth;

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

        builder.Services.AddSwaggerStuff();
        builder.Services.AddJwtAuthetication(builder.Configuration);

        builder.AddFrontEndOriginsCors();

        builder.Services.AddSingleton<DbContext, MongoDbContext>(); // Transient: instance per code request. Scoped: instance per HTTP request
        builder.Services.AddSingleton<IIssuerSigningKeyFactory, IssuerSigningKeyFactory>();
        builder.Services.AddSingleton<IPasswordHasher, AspPasswordHasher>();
        builder.Services.AddSingleton<IJwtFactory, JwtFactory>();

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
