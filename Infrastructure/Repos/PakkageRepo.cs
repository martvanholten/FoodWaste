using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repos
{
    public class PakkageRepo : IPakkageRepo
    {
        private FoodWasteContext ctx;

        public PakkageRepo(FoodWasteContext ctx)
        {
            this.ctx = ctx;
        }

        public IEnumerable<Pakkage?> GetPakkages() 
            => ctx.Pakkages.ToList();

        public IEnumerable<Pakkage?> GetPakkagesByType(string city, string cantine, string type) 
            => ctx.Pakkages
            .Where(p => p.Type.Equals(type) && p.City.Equals(city) && p.Cantine.Equals(cantine))
            .ToList();

        public IEnumerable<Pakkage?> GetPakkagesFromCantine(string city, string cantine)
            => ctx.Pakkages.Where(p => p.City.Equals(city) && p.Cantine.Equals(cantine)).ToList();

        public Pakkage? GetPakkage(string title)
            => ctx.Pakkages.Where(p => p.Title.Equals(title))
            .Include(p => p.Products).FirstOrDefault();

        public IEnumerable<Pakkage?> GetPakkagesForStudent(int studentNr)
            => ctx.Pakkages.Where(p => p.ReservedFor == null || p.ReservedFor == studentNr).ToList();

        public IEnumerable<Pakkage?> GetPakkagesReservedByStudent(int studentNr)
            => ctx.Pakkages.Where(p => p.ReservedFor == studentNr).ToList();

        public void AddPakkage(Pakkage pakkage)
        {
            ctx.Pakkages.Add(pakkage);
            ctx.SaveChanges();
        }

        public void UpdatePakkage(Pakkage pakkage)
        {
            ctx.Pakkages.Update(pakkage);
            ctx.SaveChanges();
        }

        public void RemovePakkage(Pakkage pakkage)
        {
            ctx.Pakkages.Remove(pakkage);
            ctx.SaveChanges();
        }
    }
}
