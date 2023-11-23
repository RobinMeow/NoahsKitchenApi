using System;

namespace api.Assertions;

/// <summary>
/// Validates the argument, and throws an exception when the assertion (statement) is not fullfilled. Unlike <see cref="Assert" /> this calss builds error message based on the argument name.
/// </summary>
public static class AssertArgument
{
    public static void ArgumentNotNull(object objectValue, string argumentName)
    {
        if (objectValue == null)
        {
            throw new ArgumentNullException($"Argument may not be null. '{argumentName}'", argumentName);
        }
    }

    /// <summary>throws an Exception, when the string is null, consists only of whitespace or its (trimmed) Length is zero</summary>
    public static void ArgumentNotNull(string stringValue, string argumentName)
    {
        string trimmedString = stringValue?.Trim() ?? string.Empty;

        if (String.IsNullOrWhiteSpace(trimmedString) || trimmedString.Length == 0)
        {
            throw new ArgumentNullException($"String '{argumentName}' may not be null.", argumentName);
        }
        else if (trimmedString.Length == 0)
        {
            throw new ArgumentException($"String '{argumentName}' may not be empty.", argumentName);
        }
    }

    public static void ArgumentNotNullOrDefault(object objectValue, string argumentName)
    {
        if (objectValue == null)
        {
            throw new ArgumentNullException($"Argument '{argumentName}' may not be null.", argumentName);
        }
        else if (objectValue == default)
        {
            throw new ArgumentException($"Argument '{argumentName}' may not be default.", argumentName);
        }
    }
}
