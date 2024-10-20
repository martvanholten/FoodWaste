namespace Domain.Models;

public partial class Cantine
{
    public string City { get; set; } = null!;

    public string Location { get; set; } = null!;

    public int Warm { get; set; }

    public virtual ICollection<Employ> Employs { get; set; } = new List<Employ>();

    public virtual ICollection<Pakkage> Pakkages { get; set; } = new List<Pakkage>();
}
