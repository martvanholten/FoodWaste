using System.ComponentModel.DataAnnotations;

namespace UserInterface.Validation
{
    public class ExperationDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext ctx)
        {
            if (value is DateTime)
            {
                DateTime experationDate = (DateTime)value;
                if (experationDate > DateTime.Now.AddDays(2))
                {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult(GetMsg(ctx.DisplayName ?? "Verval datum"));
        }

        private string GetMsg(string displayName)
            => $"{displayName} moet over drie dagen of later zijn";
    }
}
