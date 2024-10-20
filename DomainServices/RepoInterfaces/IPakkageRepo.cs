namespace DomainServices.RepoInterfaces
{
    public interface IPakkageRepo
    {
        public IEnumerable<Pakkage?> GetPakkages();

        public IEnumerable<Pakkage?> GetPakkagesByType(string city, string cantine, string type);

        public IEnumerable<Pakkage?> GetPakkagesFromCantine(string city, string cantine);

        public Pakkage? GetPakkage(string title);

        public IEnumerable<Pakkage?> GetPakkagesForStudent(int studentNr);

        public IEnumerable<Pakkage?> GetPakkagesReservedByStudent(int studentNr);

        public void AddPakkage(Pakkage pakkage);

        public void UpdatePakkage(Pakkage pakkage);

        public void RemovePakkage(Pakkage pakkage);
    }
}
