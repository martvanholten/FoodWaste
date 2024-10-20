namespace ApplicationServices.Interfaces
{
    public interface IPakkageService
    {
        public IEnumerable<Pakkage?> GetPakkages();

        public IEnumerable<Pakkage?> GetPakkagesByType(string city, string cantine, string type);

        public IEnumerable<Pakkage?> GetPakkagesFromCantine(string city, string cantine);

        public Pakkage GetPakkage(string title);

        public IEnumerable<Pakkage?> GetPakkagesForStudent(int studentNr);

        public IEnumerable<Pakkage?> GetPakkagesReservedByStudent(int studentNr);

        public void AddPakkage(Pakkage pakkage);

        public void UpdatePakkage(Pakkage pakkage);

        public void RemovePakkage(string title);

        public void AddProduct(string pakkageTitle, string productTitle);

        public void RemoveProduct(string pakkageTitle, string productTitle);
    }
}
