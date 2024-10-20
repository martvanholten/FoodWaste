using System.ComponentModel.DataAnnotations;

namespace UserInterface.Validation
{
    public class TypeAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext ctx)
        {
            if (value is string)
            {
                string text = (string)value;
                if (text.Equals("Ontbijt") || text.Equals("Lunch") || text.Equals("Avond maal"))
                {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult(GetMsg(ctx.DisplayName ?? "Type"));
        }

        private string GetMsg(string displayName)
            => $"{displayName} moet Ontbijt, Lunch of Avond maal";
    }
}
