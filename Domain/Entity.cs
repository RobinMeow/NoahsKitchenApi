namespace api.Domain;

public abstract class Entity
{
    public Entity(EntityId entityId)
    {
        this.Id = entityId;
    }

    public EntityId Id { get; private set; } = null!; // Initially there was no setter, but MongoDb depends on it. Semms to work with private tho, unlike EFCore.

    public abstract int ModelVersion { get; init; } // start at zero, so the version is also the amount of times, it was changed :)

    IsoDateTime _createdAt = IsoDateTime.Now;

    public IsoDateTime CreatedAt
    {
        get => _createdAt;
        set => _createdAt = value;
    }
}
