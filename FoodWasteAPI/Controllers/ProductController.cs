using FoodWasteAPI.DTO;
using FoodWasteAPI.ErrorHandeler;
using Microsoft.AspNetCore.Authorization;

namespace FoodWasteAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        //GET ALL PRODUCTS
        [HttpGet]
        [Authorize]
        [Route("allen")]
        public IActionResult All()
        {
            try
            {
                return Ok(productService.GetProducts());
            }
            catch (Exception ex)
            {
                if (ex is ErrorModel)
                {
                    ErrorModel er = (ErrorModel)ex;
                    return BadRequest($"Iets is er mis gegaan: {er.Error}");
                    //return HttpErrorHandler.GetHttpError(er);
                }
                return BadRequest($"Iets is er mis gegaan");
            }
        }

        //GET ONE PRODUCT
        [HttpGet]
        [Authorize]
        [Route("{title}")]
        public IActionResult Product(string title)
        {
            try
            {
                Product product = productService.GetProduct(title);
                ProductDTO productDTO = new ProductDTO()
                {
                    Title = product.Title,
                    Photo = product.Photo,
                    Alchol = product.Alchol,
                };
                return Ok(productDTO);
            }
            catch (Exception ex)
            {
                if (ex is ErrorModel)
                {
                    ErrorModel er = (ErrorModel)ex;
                    return BadRequest($"Iets is er mis gegaan: {er.Error}");
                    //return HttpErrorHandler.GetHttpError(er);
                }
                return BadRequest($"Iets is er mis gegaan");
            }
        }
    }
}
