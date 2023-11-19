using System;

namespace api.Assertions;

/// <summary>Throw an <see cref="InvalidOperationException"> when the assertion (statement) is not fullfilled and adds the passed in message to it.</summary>
public static class Assert
{
    public static void True(bool boolValue, string message)
    {
        if (!boolValue)
        {
            throw new InvalidOperationException(message);
        }
    }

    public static void False(bool boolValue, string message)
    {
        True(!boolValue, message);
    }

    public static void NotNull(object objectValue, string message)
    {
        if (objectValue == null)
        {
            throw new InvalidOperationException(message);
        }
    }

    public static void NotNullOrEmpty(string stringValue, string message)
    {
        string trimmedString = stringValue.Trim();
        if (String.IsNullOrWhiteSpace(trimmedString) || trimmedString.Length == 0)
        {
            throw new InvalidOperationException(message);
        }
    }
}
