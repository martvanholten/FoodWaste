using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class Student
{
    public int StudentNr { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string City { get; set; } = null!;

    public int Phonenumber { get; set; }

    public virtual ICollection<Pakkage> Pakkages { get; set; } = new List<Pakkage>();
}
