using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum enumValue)
    {
        string? displayName;
        displayName = enumValue.GetType()
            .GetMember(enumValue.ToString())
            .FirstOrDefault()
            .GetCustomAttribute<DisplayAttribute>()?
            .GetName();
        if (String.IsNullOrEmpty(displayName))
        {
            displayName = enumValue.ToString();
        }
        return displayName;
    }
}

public class ExpireTimeValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        value = (DateTime)value;
        // This assumes inclusivity, i.e. exactly six years ago is okay
        if (DateTime.Today.CompareTo(value) < 0)
        {
            bool isGreeter = DateTime.Today.CompareTo(value) > 0;
            return ValidationResult.Success;
        }
        else
        {
            return new ValidationResult("Срок действия должен быть позже сегодняшнего дня!");
        }
    }
}