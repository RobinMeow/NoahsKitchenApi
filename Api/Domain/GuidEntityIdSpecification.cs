namespace api.Domain;

public sealed class GuidEntityIdSpecification : IdentifierSpecification
{
    /// <summary>The format for representing a GUID as a string.</summary>
    public const string GuidFormat = "D";

    public static readonly string[] DisallowedIds = {
        "00000000-0000-0000-0000-000000000000",
        "ffffffff-ffff-ffff-ffff-ffffffffffff",
        "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF"
    };

    public static bool IsValidGuidFormat(string id)
    {
        return Guid.TryParseExact(id, GuidFormat, out _);
    }

    public static bool IsDisallowedId(string id)
    {
        for (int i = 0; i < DisallowedIds.Length; i++)
            if (DisallowedIds[i] == id)
                return true;
        return false;
    }

    public bool IsSatisfiedBy(string guid)
    {
        if (string.IsNullOrWhiteSpace(guid))
            return false;
        // throw new ArgumentNullException(nameof(guid), $"{nameof(guid)} cannot be null or white space.");

        if (!IsValidGuidFormat(guid))
            return false;
        // throw new ArgumentException($"{nameof(guid)} must be in the '{GuidFormat}' formatted GUID in all lowercase.");

        if (IsDisallowedId(guid))
            return false;
        // throw new ArgumentException($"{nameof(guid)} cannot be a disallowed GUID.");

        return true;
    }
}
