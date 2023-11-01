using System;
using System.Globalization;

namespace api.Domain;

public sealed class IsoDateTime
{
    static readonly CultureInfo InvariantCulture = CultureInfo.InvariantCulture; // cache for performance

    ///<summary>Array of allowed ISO 8601 date-time formats ordered in likelihood to occur.</summary>
    static readonly string[] AllowedIsoFormats = new[]
    {
        "yyyy-MM-dd'T'HH:mm:ss.fffffff'Z'",
        "yyyy-MM-dd'T'HH:mm:ss.ffffff'Z'",
        "yyyy-MM-dd'T'HH:mm:ss.fffff'Z'",
        "yyyy-MM-dd'T'HH:mm:ss.ffff'Z'",
        "yyyy-MM-dd'T'HH:mm:ss.fff'Z'",
        "yyyy-MM-dd'T'HH:mm:ss.ff'Z'",
        "yyyy-MM-dd'T'HH:mm:ss.f'Z'",
        "yyyy-MM-dd'T'HH:mm:ss'Z'"
    };

    DateTime _dateTime = DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);

    private IsoDateTime()
    {
    }

    public static IsoDateTime Now
    {
        get
        {
            return new IsoDateTime(DateTime.UtcNow);
        }
    }

    /// <summary>
    /// Converts the specified string in ISO 8601 format to a <see cref="DateTime"/> with <see cref="DateTimeKind.Utc"/>.
    /// <para>The provided string has to be in a format specified in <see cref="AllowedIsoFormats"/></para>
    /// <code>
    /// yyyy-MM-dd'T'HH:mm:ss.fffffff'Z' <br />
    /// yyyy-MM-dd'T'HH:mm:ss.ffffff'Z' <br />
    /// yyyy-MM-dd'T'HH:mm:ss.fffff'Z' <br />
    /// yyyy-MM-dd'T'HH:mm:ss.ffff'Z' <br />
    /// yyyy-MM-dd'T'HH:mm:ss.fff'Z' <br />
    /// yyyy-MM-dd'T'HH:mm:ss.ff'Z' <br />
    /// yyyy-MM-dd'T'HH:mm:ss.f'Z' <br />
    /// yyyy-MM-dd'T'HH:mm:ss'Z' <br />
    /// </code>
    /// </summary>
    public IsoDateTime(string isoString)
    {
        DateTime dateTimeUnspecified = DateTime.ParseExact(isoString, AllowedIsoFormats, InvariantCulture, DateTimeStyles.AdjustToUniversal);
        _dateTime = DateTime.SpecifyKind(dateTimeUnspecified, DateTimeKind.Utc);
    }

    public IsoDateTime(DateTime dateTime)
    {
        if (dateTime.Kind != DateTimeKind.Utc)
            throw new ArgumentException("DateTimeKind has to be in Utc.", nameof(dateTime));

        _dateTime = dateTime;
    }

    public static implicit operator DateTime(IsoDateTime isoDateTime)
    {
        return isoDateTime._dateTime;
    }

    /// <summary>
    /// Converts <see cref="_dateTime"/> to an ISO 8601 string representation in UTC <see cref="DateTimeKind"/>.
    /// <para>
    /// Example: 2023-06-14T12:34:56.1234567Z<br />
    /// Format: yyyy-MM-dd'T'HH:mm:ss.fffffff'Z'
    /// </para>
    /// </summary>
    public override string ToString()
    {
        return _dateTime.ToString("o", InvariantCulture);
    }
}
