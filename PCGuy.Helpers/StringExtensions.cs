namespace PCGuy.Helpers;

public static class StringExtensions
{
    public static string ToImageAltText(this string name)
    {
        return string.IsNullOrEmpty(name) ? " current product" : name.ToLowerInvariant().Replace(" ", string.Empty).Trim();
    }
}