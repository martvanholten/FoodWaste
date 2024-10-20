namespace DomainServices.RepoInterfaces
{
    public interface IProductRepo
    {
        public Product? GetProduct(string title);

        public IEnumerable<Product?> GetProducts();
    }
}
