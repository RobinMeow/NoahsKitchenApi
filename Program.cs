using api.Domain;
using api.Infrastructure;

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
