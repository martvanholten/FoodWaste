namespace Domain.Models;

public partial class Employ
{
    public int EmployNr { get; set; }

    public string Name { get; set; } = null!;

    public string Cantine { get; set; } = null!;

    public string City { get; set; } = null!;

    public virtual Cantine CantineNavigation { get; set; } = null!;
}
