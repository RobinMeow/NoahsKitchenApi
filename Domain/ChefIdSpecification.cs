namespace api.Domain;

public sealed class ChefIdSpecification : IdentifierSpecification
{
    public bool IsSatisfiedBy(string chefId)
    {
        // https://firebase.google.com/docs/auth/admin/manage-users#create_a_user (For validations)
        if (string.IsNullOrWhiteSpace(chefId))
            return false;

        if (chefId.Length > 128 || chefId.Length < 1)
            return false;

        return true;
    }
}

