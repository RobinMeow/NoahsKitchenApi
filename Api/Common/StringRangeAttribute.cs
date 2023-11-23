using System.ComponentModel.DataAnnotations;

namespace api;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class StringRangeAttribute : StringLengthAttribute
{
    public StringRangeAttribute(int minimumLength, int maximumLength) : base(maximumLength)
    {
        MinimumLength = minimumLength;
    }
}
