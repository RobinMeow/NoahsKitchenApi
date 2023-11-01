using api.Domain;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace api.Infrastructure;

public sealed class MongoDbContext : DbContext
{
    public override IRecipeRepository RecipeRepository { get; init; }

    public override IChefRepository ChefRepository { get; init; }

    public MongoDbContext(IOptions<PersistenceSettings> persistenceSettings)
    : base()
    {
        ConventionPack camelCaseConvention = new ConventionPack {
            new CamelCaseElementNameConvention()
        };
        ConventionRegistry.Register("CamelCase", camelCaseConvention, (System.Type type) => true);

        if (!BsonClassMap.IsClassMapRegistered(typeof(Recipe))) // ToDo: Check where this call belongs
        {
            BsonSerializer.RegisterSerializer(typeof(IsoDateTime), new BsonIsoDateTimeSerializer()); // Serializers (in expectation to have the same lifetime scope as ClassMaps)

            BsonClassMap.RegisterClassMap<Entity>(x =>
            {
                x.AutoMap();
                x.MapMember(entity => entity.ModelVersion).SetElementName("__v");
                x.MapMember(entity => entity.Id).SetElementName("_id").SetSerializer(new EntityIdSerializer());
            });

            BsonClassMap.RegisterClassMap<Chef>(x =>
            {
                x.SetDiscriminator(nameof(Entity));
                x.AutoMap();
            });

            BsonClassMap.RegisterClassMap<Recipe>(x =>
            {
                x.SetDiscriminator(nameof(Entity));
                x.AutoMap();
            });
        }

        MongoClientSettings settings = MongoClientSettings.FromConnectionString(persistenceSettings.Value.ConnectionString);
        settings.ServerApi = new ServerApi(ServerApiVersion.V1);
        MongoClient client = new MongoClient(settings);

        string databaseName = Globals.ApplicationNameAbbreviated.ToLower();

        IMongoDatabase db = client.GetDatabase(databaseName);

        RecipeRepository = new RecipeMongoDbCollection(db);
        ChefRepository = new ChefMongoDbCollection(db);
    }
}

// Help:
// http://mongodb.github.io/mongo-csharp-driver/2.2/reference/bson/mapping/
// http://mongodb.github.io/mongo-csharp-driver/2.2/reference/bson/mapping/#mapping-classes
// https://www.mongodb.com/docs/drivers/csharp/current/fundamentals/class-mapping/
