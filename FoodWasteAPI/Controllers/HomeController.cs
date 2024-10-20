using FoodWasteAPI.DTO;
using FoodWasteAPI.ErrorHandeler;
using Microsoft.AspNetCore.Authorization;

namespace FoodWasteAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private IEmailService emailService;
        private IUserService userService;

        public HomeController(IUserService userService, IEmailService emailService)
        {
            this.userService = userService;
            this.emailService = emailService;
        }

        //REGISTER USER
        [HttpPost]
        [Route("/home/register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var code = await userService.Register(registerDTO.Name, registerDTO.Id,
                        registerDTO.Password, registerDTO.Email, registerDTO.Role);

                    if (code != null)
                    {
                        if (await userService.VerifyEmail(registerDTO.Email, registerDTO.Password, code))
                        {
                            return Ok($"Gebruiker {registerDTO.Id} aangemaakt");
                        }

                        return BadRequest();
                    }

                    return BadRequest();
                }
                else
                {
                    return BadRequest(registerDTO);
                }
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
        
        //LOGIN
        [HttpPut]
        [Route("/home/gebruiker/login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await userService.Login(loginDTO.Id, loginDTO.Password))
                    {
                        return Ok($"In gelod met gebruiker {loginDTO.Id}");
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest(loginDTO);
                }
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

        //LOGOUT
        [Authorize]
        [HttpPut]
        [Route("/home/gebruiker/loguit")]
        public IActionResult LogOut()
        {
            try
            {
                userService.LogOut();
                return Ok("Uit gelogd");
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