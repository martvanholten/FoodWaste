namespace ApplicationServices.Interfaces
{
    public interface IProductService
    {
        public Product GetProduct(string title);

        public IEnumerable<Product?> GetProducts();
    }
}
