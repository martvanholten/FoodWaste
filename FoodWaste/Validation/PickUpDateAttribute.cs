using System.ComponentModel.DataAnnotations;

namespace UserInterface.Validation
{
    public class PickUpDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext ctx)
        {
            if (value is DateTime)
            {
                DateTime pickUpDate = (DateTime)value;
                if (pickUpDate >= DateTime.Now && pickUpDate < DateTime.Now.AddDays(3))
                {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult(GetMsg(ctx.DisplayName ?? "Ophaal datum"));
        }

        private string GetMsg(string displayName)
            => $"{displayName} moet in de komende twee dagen zijn";
    }
}
