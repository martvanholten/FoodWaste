using Infrastructure.Data;

namespace Infrastructure.Repos
{
    public class CantineRepo : ICantineRepo
    {
        private FoodWasteContext ctx;

        public CantineRepo(FoodWasteContext ctx)
        {
            this.ctx = ctx;
        }
        public IEnumerable<Cantine?> GetCantines() => ctx.Cantines.ToList();

        public IEnumerable<IGrouping<string?, Cantine?>> GetCitys() => ctx.Cantines.GroupBy(c => c.City).ToList();

        public IEnumerable<Cantine?> GetCantinesByCity(string city) => ctx.Cantines.Where(c => c.City.Equals(city)).ToList();

        public Cantine? GetCantine(string city, string location) => ctx.Cantines.
            Where(c => c.City.Equals(city) && c.Location.Equals(location)).FirstOrDefault();
    }
}
