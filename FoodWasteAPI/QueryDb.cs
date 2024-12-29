using HotChocolate.Types;

namespace FoodWasteAPI
{
    public class QueryDb
    {
        [UsePaging]
        public IQueryable<Pakkage> getPakkages(FoodWasteContext context) 
            => context.Pakkages;
    }
}
