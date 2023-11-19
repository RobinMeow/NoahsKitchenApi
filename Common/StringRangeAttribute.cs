using System.ComponentModel.DataAnnotations;

namespace api;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class StringRangeAttribute : StringLengthAttribute
{
    public StringRangeAttribute(int maximumLength, int minimumLength) : base(maximumLength)
    {
        MinimumLength = minimumLength;
    }
}
