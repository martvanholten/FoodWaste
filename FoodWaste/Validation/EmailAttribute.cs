using System.ComponentModel.DataAnnotations;

namespace UserInterface.Validation
{
    public class EmailAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext ctx)
        {
            if (value is string)
            {
                string email = (string)value;
                string[] parts = email.Split("@");
                return parts.Length != 2 ? new ValidationResult(GetMsg(ctx.DisplayName ?? "Email")) 
                    : parts[1].Split(".").Length == 2 ? ValidationResult.Success
                    : new ValidationResult(GetMsg(ctx.DisplayName ?? "Email"));
            }
            return new ValidationResult(GetMsg(ctx.DisplayName ?? "Email"));
        }

        private string GetMsg(string displayName)
            => $"{displayName} moet een email zijn";
    }
}
