namespace ApplicationServices.Interfaces
{
    public interface ICantineService
    {
        public Cantine GetCantine(string city, string cantine);

        public IEnumerable<Cantine?> GetCantines();

        public IEnumerable<Cantine?> GetCantinesByCity(string city);

        public IEnumerable<IGrouping<string?, Cantine?>> GetCitys();
    }
}
