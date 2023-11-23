namespace api.Domain;

public interface IdentifierSpecification
{
    public bool IsSatisfiedBy(string identifier);
}
