﻿using System.ComponentModel.DataAnnotations;
using UserInterface.Validation;

namespace UserInterface.ViewModels
{
    public class ViewUpdatePakkage
    {
        [Display(Name = "Titel")]
        public string? Title { get; set; }

        [Display(Name = "Stad")]
        public string? City { get; set; }

        [Display(Name = "Cantine")]
        public string? Cantine { get; set; }

        [Display(Name = "Ophaal datum")]
        [PickUpDate(ErrorMessage = "De ophaal datum moet binnen twee dagen zijn")]
        public DateTime? PickUpDate { get; set; }

        [Display(Name = "Verval datum")]
        [ExperationDate(ErrorMessage = "De verval datum moet verder dan twee dagen zijn")]
        public DateTime? ExperationDate { get; set; }

        [Display(Name = "Leeftijds restrictie")]
        public int? AgeRestriction { get; set; }

        [Display(Name = "Prijs")]
        [PositiveNumber(ErrorMessage = "De prijs moet hoger zijn dan 0")]
        public decimal? Price { get; set; }

        [Display(Name = "Type")]
        [Type(ErrorMessage = "Het type moet Ontbijt, Lunch of Aavond maal zijn")]
        public string? Type { get; set; }

        [Display(Name = "Gereserveerd voor")]
        public int? ReservedFor { get; set; }

        [Display(Name = "Producten")]
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
