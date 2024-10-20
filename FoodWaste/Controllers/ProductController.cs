using Microsoft.AspNetCore.Authorization;

namespace UserInterface.Controllers
{
    public class ProductController : Controller
    {
        private IProductService productService;

        public ProductController(IProductService productService) {
            this.productService = productService;
        }

        //GET ALL PRODUCTS
        [HttpGet]
        [Authorize]
        public IActionResult All()
        {
            try
            {
                return View("products", productService.GetProducts());
            }
            catch (Exception ex)
            {
                if (ex is ErrorModel)
                {
                    ErrorModel er = (ErrorModel)ex;
                    return View("ErrorView", $"Iets is er mis gegaan: {er.Error}");
                }
                return View("ErrorView", $"Iets is er mis gegaan");
            }
        }

        //GET ONE PRODUCT
        [HttpGet]
        [Authorize]
        public IActionResult Product(string title)
        {
            try
            {
                return View("product", productService.GetProduct(title));
            }
            catch (Exception ex)
            {
                if (ex is ErrorModel)
                {
                    ErrorModel er = (ErrorModel)ex;
                    return View("ErrorView", $"Iets is er mis gegaan: {er.Error}");
                }
                return View("ErrorView", $"Iets is er mis gegaan");
            }
        }
    }
}
