using Infrastructure.Data;

namespace Infrastructure.Repos
{
    public class EmployRepo : IEmployRepo
    {
        private FoodWasteContext ctx;

        public EmployRepo(FoodWasteContext ctx)
        {
            this.ctx = ctx;
        }
        public IEnumerable<Employ?> GetEmploys() => ctx.Employs.ToList();
    }
}
