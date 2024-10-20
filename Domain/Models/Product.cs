namespace Domain.Models;

public partial class Product
{
    public string Title { get; set; } = null!;

    public string Photo { get; set; } = null!;

    public int Alchol { get; set; }

    public virtual ICollection<Pakkage> Pakkages { get; set; } = new List<Pakkage>();
}
