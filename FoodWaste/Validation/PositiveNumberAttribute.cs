using System.ComponentModel.DataAnnotations;

namespace UserInterface.Validation
{
    public class PositiveNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext ctx)
        {
            if (value is int)
            {
                int number = (int)value;
                if (number > 0)
                {
                    return ValidationResult.Success;
                }
            }
            else if (value is decimal)
            {
                decimal number = (decimal)value;
                if (number > 0)
                {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult(GetMsg(ctx.DisplayName ?? "Nummer"));
        }

        private string GetMsg(string displayName)
            => $"{displayName} moet een getal en hoger zijn dan 0";
    }
}
