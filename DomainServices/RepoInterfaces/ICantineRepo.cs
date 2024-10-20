namespace DomainServices.RepoInterfaces
{
    public interface ICantineRepo
    {
        public Cantine? GetCantine(string city, string location);

        public IEnumerable<Cantine?> GetCantines();
        
        public IEnumerable<IGrouping<string?, Cantine?>> GetCitys();

        public IEnumerable<Cantine?> GetCantinesByCity(string city);
    }
}
