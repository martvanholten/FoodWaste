namespace ApplicationServices.Logic
{
    public class ProductService : IProductService
    {
        private IProductRepo productRepo;

        public ProductService(IProductRepo productRepo) { 
            this.productRepo = productRepo;
        }

        public Product GetProduct(string title)
        {
            try
            {
                Product? product = productRepo.GetProduct(title);
                if (product == null)
                {
                    throw new ErrorModel($"Het product {title} bestaat niet", 400);
                }
                else
                {
                    return product;
                }
            }
            catch (Exception ex)
            {
                if (ex is ErrorModel)
                {
                    ErrorModel er = (ErrorModel)ex;
                    throw er;
                }
                throw new ErrorModel("Database fout", 500);
            }
        }

        public IEnumerable<Product?> GetProducts()
        {
            try
            {
                return productRepo.GetProducts();
            }
            catch
            {
                throw new ErrorModel("Database fout", 500);
            }
        }
    }
}
