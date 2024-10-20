using Microsoft.AspNetCore.Identity;

namespace UserInterface.ViewModels
{
    public class StudentNrPakkages
    {
        public CantinePakkages CantinePakkages { get; set; }
        public int StudentNr { get; set; }

        public StudentNrPakkages()
        {
            this.CantinePakkages = new CantinePakkages();
        }
    }
}
