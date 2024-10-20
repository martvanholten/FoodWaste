namespace UserInterface.ViewModels
{
    public class CantinePakkages
    {
        public IEnumerable<Pakkage?>? Pakkages { get; set; }

        public IEnumerable<IGrouping<string?, Cantine?>>? City { get; set; }

        public IEnumerable<Cantine?>? Cantine { get; set; }
    }
}
