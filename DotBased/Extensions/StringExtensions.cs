namespace DotBased.Extensions;

/// <summary>
/// Some simple extensions used for the string class
/// </summary>
public static class StringExtensions
{
    public static bool IsNullOrWhiteSpace(this string s) => string.IsNullOrWhiteSpace(s);
}