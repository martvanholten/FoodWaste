using System.ComponentModel.DataAnnotations;

namespace FoodWasteAPI.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Het student/medewerker nummer is verplicht")]
        public string Id { get; set; } = null!;

        [Required(ErrorMessage = "Het wachtwoord is verplicht")]
        public string Password { get; set; } = null!;
    }
}
