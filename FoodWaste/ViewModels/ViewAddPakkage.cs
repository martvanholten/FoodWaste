using System.ComponentModel.DataAnnotations;
using UserInterface.Validation;

namespace UserInterface.ViewModels
{
    public class ViewAddPakkage
    {
        [Required(ErrorMessage = "De titel is verplicht")]
        [Display(Name = "Titel")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "De stad is verplicht")]
        [Display(Name = "Stad")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "De cantine is verplicht")]
        [Display(Name = "Cantine")]
        public string Cantine { get; set; } = null!;

        [Required(ErrorMessage = "De ophaal datum is verplicht")]
        [PickUpDate(ErrorMessage="De ophaal datum moet binnen twee dagen zijn")]
        [Display(Name = "Ophaal datum")]
        public DateTime PickUpDate { get; set; }

        [Required(ErrorMessage = "De verval datum is verplicht")]
        [ExperationDate(ErrorMessage = "De verval datum moet verder dan twee dagen zijn")]
        [Display(Name = "Verval datum")]
        public DateTime ExperationDate { get; set; }

        public int AgeRestriction { get; set; }

        [Required(ErrorMessage = "De prijs is verplicht")]
        [PositiveNumber(ErrorMessage = "De prijs moet hoger zijn dan 0")]
        [Display(Name = "Prijs")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Het type is verplicht")]
        [Type(ErrorMessage = "Het type moet Ontbijt, Lunch of Aavond maal zijn")]
        [Display(Name = "Type")]
        public string Type { get; set; } = null!;

        public int? ReservedFor { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
