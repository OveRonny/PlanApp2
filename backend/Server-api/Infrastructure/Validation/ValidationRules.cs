namespace Server_api.Infrastructure.Validation;

public static class ValidationRules
{
    public const int NameMaxLength = 150;
    public const int DescriptionMaxLength = 2000;

    public static string? NameError(string? value, string entityName)
    {
        var name = value?.Trim();
        return string.IsNullOrWhiteSpace(name) || name.Length > NameMaxLength
            ? $"{entityName} name must be between 1 and {NameMaxLength} characters."
            : null;
    }

    public static string? DescriptionError(string? value, string entityName)
    {
        return value?.Length > DescriptionMaxLength
            ? $"{entityName} description cannot exceed {DescriptionMaxLength} characters."
            : null;
    }
}
