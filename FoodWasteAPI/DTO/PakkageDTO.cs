namespace FoodWasteAPI.DTO
{
    public class PakkageDTO
    {
        public string Title { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Cantine { get; set; } = null!;

        public DateTime PickUpDate { get; set; }

        public DateTime ExperationDate { get; set; }

        public int AgeRestriction { get; set; }

        public decimal Price { get; set; }

        public string Type { get; set; } = null!;

        public int? ReservedFor { get; set; }

        public virtual Cantine CantineNavigation { get; set; } = null!;

        public virtual Student? ReservedForNavigation { get; set; }
    }
}
