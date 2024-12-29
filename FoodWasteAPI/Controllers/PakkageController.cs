using FoodWasteAPI.DTO;
using FoodWasteAPI.ErrorHandeler;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;

namespace FoodWasteAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PakkageController : ControllerBase
    {
        private IUserService userService;
        private IPakkageService pakkageService;
        private IProductService productService;
        private IStudentService studentService;
        private ICantineService cantineService;

        public PakkageController(IUserService userService, IPakkageService pakkageService,
            IProductService productService, IStudentService studentService, ICantineService cantineService)
        {
            this.userService = userService;
            this.pakkageService = pakkageService;
            this.productService = productService;
            this.studentService = studentService;
            this.cantineService = cantineService;
        }

        //GET ALL PAKKAGES
        [Authorize]
        [HttpGet]
        [Route("/pakkage/allen")]
        public IActionResult Pakkages()
        {
            try
            {
                return Ok(pakkageService.GetPakkages());
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

        //GET PAKKAGES RESERVED BY STUDENT
        [Authorize(Roles = "Student")]
        [HttpGet]
        [Route("/pakkage/student/{id}")]
        public IActionResult Student(string id)
        {
            try
            {
                int studentNr = int.Parse(id);
                return Ok(pakkageService.GetPakkagesReservedByStudent(studentNr));
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

        //GET ONE PAKKAGE
        [HttpGet]
        [Authorize(Roles = "Employ, Student")]
        [Route("/pakkage/{title}")]
        public IActionResult Pakkage(string title)
        {
            try
            {
                var pakkage = pakkageService.GetPakkage(title);
                PakkageDTO pakkageDTO = new PakkageDTO()
                {
                    Title = pakkage.Title,
                    Type = pakkage.Type,
                    AgeRestriction = pakkage.AgeRestriction,
                    City = pakkage.City,
                    Cantine = pakkage.Cantine,
                    CantineNavigation = pakkage.CantineNavigation,
                    PickUpDate = pakkage.PickUpDate,
                    ExperationDate = pakkage.ExperationDate,
                    Price = pakkage.Price,
                    ReservedFor = pakkage.ReservedFor,
                    ReservedForNavigation = pakkage.ReservedForNavigation,
                };
                return Ok(pakkageDTO);
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

        //UPDATE ONE PAKKAGE
        [HttpPut]
        [Authorize(Roles = "Employ")]
        [Route("/pakkage/{oldTitle}")]
        public IActionResult Update(UpdatePakkageDTO pakkageDTO, string oldTitle)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(pakkageDTO);
                }
                Pakkage pakkage = pakkageService.GetPakkage(oldTitle);
                Cantine cantine = cantineService.GetCantine
                        (pakkageDTO.City ?? pakkage.City, pakkageDTO.Cantine ?? pakkage.Cantine);
                pakkage.City = pakkageDTO.City ?? pakkage.City;
                pakkage.Cantine = pakkageDTO.Cantine ?? pakkage.Cantine;
                pakkage.PickUpDate = pakkageDTO.PickUpDate ?? pakkage.PickUpDate;
                pakkage.ExperationDate = pakkageDTO.ExperationDate ?? pakkage.ExperationDate;
                pakkage.AgeRestriction = pakkageDTO.AgeRestriction ?? pakkage.AgeRestriction;
                pakkage.Price = pakkageDTO.Price ?? pakkage.Price;
                pakkage.Type = pakkageDTO.Type ?? pakkage.Type;
                pakkage.CantineNavigation = cantine;
                                
                pakkageService.UpdatePakkage(pakkage);
                return Ok(pakkage);
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

        //DELETE ONE PAKKAGE
        [HttpDelete]
        [Authorize(Roles = "Employ")]
        [Route("/pakkage/{title}")]
        public IActionResult Delete(string title)
        {
            try
            {
                pakkageService.RemovePakkage(title);
                return Ok(pakkageService.GetPakkages());
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

        //ADD ONE PAKKAGE
        [HttpPost]
        [Authorize(Roles = "Employ")]
        [Route("/pakkage")]
        public IActionResult Add(AddPakkageDTO pakkageDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Cantine cantine = cantineService.GetCantine
                        (pakkageDTO.City, pakkageDTO.Cantine);
                    Pakkage pakkage = new Pakkage()
                    {
                        Title = pakkageDTO.Title,
                        City = pakkageDTO.City,
                        Cantine = pakkageDTO.Cantine,
                        PickUpDate = pakkageDTO.PickUpDate,
                        ExperationDate = pakkageDTO.ExperationDate,
                        AgeRestriction = 0,
                        Price = pakkageDTO.Price,
                        Type = pakkageDTO.Type,
                    };

                    pakkageService.AddPakkage(pakkage);

                    return Ok(pakkageService.GetPakkage(pakkage.Title));
                }
                return BadRequest(pakkageDTO);
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

        //ADD ONE PRODUCT TO PAKKAGE
        [HttpPost]
        [Authorize(Roles = "Employ")]
        [Route("/pakkage/{pakkageTitle}/product/{productTitle}")]
        public IActionResult AddProduct(string pakkageTitle, string productTitle)
        {
            try
            {
                pakkageService.AddProduct(pakkageTitle, productTitle);
                return Ok(pakkageService.GetPakkage(pakkageTitle));
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

        //DELETE ONE PRODUCT FROM PAKKAGE
        [HttpDelete]
        [Authorize(Roles = "Employ")]
        [Route("/pakkage/{pakkageTitle}/product/{productTitle}")]
        public IActionResult DeleteProduct(string pakkageTitle, string productTitle)
        {
            try
            {
                pakkageService.RemoveProduct(pakkageTitle, productTitle);
                return Ok(pakkageService.GetPakkage(pakkageTitle));
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

        //RESERVE A PAKKAGE
        [HttpPut]
        [Authorize(Roles = "Student")]
        [Route("/pakkage/{title}/reserveer")]
        public IActionResult Reserve(string title)
        {
            try
            {
                studentService.ReservePakkage(title, User.Identity.GetUserId());
                return Ok(pakkageService.GetPakkage(title));
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

        //GET PAKKAGES FROM CANTINE
        [HttpGet]
        [Authorize(Roles = "Employ, Student")]
        [Route("/pakkage/stad/{city}/cantine/{cantine}")]
        public IActionResult CantinePakkages(string city, string cantine)
        {
            try
            {
                return Ok(pakkageService.GetPakkagesFromCantine(city, cantine));
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

        //GET PAKKAGES FROM CANTINE FILTERED BY TYPE
        [HttpGet]
        [Authorize(Roles = "Employ, Student")]
        [Route("/pakkage/type/{type}")]
        public IActionResult Filtered(string type)
        {
            try
            {
                var pakkages = pakkageService.GetPakkages();
                return Ok(pakkages.Where(p => p!.Type.Equals(type)));
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