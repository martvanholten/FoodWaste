using Infrastructure.Data;

namespace Infrastructure.Repos
{
    public class ProductRepo : IProductRepo
    {
        private FoodWasteContext ctx;

        public ProductRepo(FoodWasteContext ctx)
        {
            this.ctx = ctx;
        }

        public Product? GetProduct(string title) => ctx.Products.Where(p => p.Title.Equals(title)).FirstOrDefault();

        public IEnumerable<Product?> GetProducts() => ctx.Products.ToList();
    }
}
