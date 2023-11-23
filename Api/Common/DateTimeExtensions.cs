using System;

namespace api;

public static class DateTimeExtensions
{
    public static string ToISOString(this DateTime date)
    {
        if (date.Kind == DateTimeKind.Utc)
            return date.ToString("o", System.Globalization.CultureInfo.InvariantCulture);

        return date.ToUniversalTime().ToString("o", System.Globalization.CultureInfo.InvariantCulture);
    }
}
