using System.ComponentModel.DataAnnotations;
using UserInterface.Validation;

namespace FoodWasteAPI.DTO
{
    public class RegisterDTO
    {
        [Display(Name = "Naam")]
        [Required(ErrorMessage = "De naam is verplicht")]
        public string Name { get; set; } = null!;

        [Display(Name = "Email")]
        [Email(ErrorMessage = "De email moet een email zijn")]
        [Required(ErrorMessage = "De email is verplicht")]
        public string Email { get; set; } = null!;

        [Display(Name = "Wachtwoord")]
        [Required(ErrorMessage = "Het wachtwoord is verplicht")]
        public string Password { get; set; } = null!;

        [Display(Name = "Rol")]
        [Required(ErrorMessage = "De rol is verplicht")]
        public string Role { get; set; } = null!;

        [Display(Name = "Student/Medewerker nummer")]
        [Required(ErrorMessage = "Het student/medewerker nummer is verplicht")]
        public string Id { get; set; } = null!;
    }
}
