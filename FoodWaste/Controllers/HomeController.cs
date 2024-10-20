using Microsoft.AspNetCore.Authorization;

namespace UserInterface.Controllers
{
    public class HomeController : Controller
    {
        private IEmailService emailService;
        private IUserService userService;

        public HomeController(IUserService userService, IEmailService emailService)
        {
            this.userService = userService;
            this.emailService = emailService;
        }

        //GET HOME VIEW
        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                return View();
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

        //GET REGISTER VIEW
        [HttpGet]
        public IActionResult Register()
        {
            try
            {
                return View();
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

        //REGISTER USER
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var code = await userService.Register(registerModel.Name, registerModel.Id,
                        registerModel.Password, registerModel.Email, registerModel.Role);

                    if (code != null)
                    {
                        //USE FOR CLIENT SIDE
                        //var link = Url.Action(nameof(VerifyEmail),
                        //   "Home", new { registerModel.Email, registerModel.Password, code }, Request.Scheme, Request.Host.ToString())!;

                        //var mailData = new MailData()
                        //{
                        //    EmailToId = registerModel.Email,
                        //    EmailSubject = "Verify",
                        //    EmailBody = link,
                        //    EmailToName = registerModel.Name,
                        //};

                        //await emailService.SendEmailAsync(mailData);

                        //return View("EmailVerification");

                        if (await userService.VerifyEmail(registerModel.Email, registerModel.Password, code))
                        {
                            return View("VerifyEmail");
                        }
                    }

                    return View("ErrorView", "Er is iets mis gegaan");
                }
                else
                {
                    return View();
                }
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

        //GET VIEW AFTER CLICKING THE EMAIL
        //USE FOR CLIENT SIDE
        //[HttpGet]
        //public async Task<IActionResult> VerifyEmail(string email, string password, string code)
        //{
        //    try
        //    {
        //        if (await userService.VerifyEmail(email, password, code))
        //        {
        //            return View("VerifyEmail");
        //        }
        //        return RedirectToAction("Error", "Home");
        //    }
        //    catch (Exception ex)
        //    {
        //        return View("ErrorHandler", ex);
        //    }
        //}

        //GET LOGIN VIEW
        [HttpGet]
        public IActionResult Login()
        {
            try
            {
                return View();
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

        //LOGIN
        [HttpPost]
        public async Task<IActionResult> Login(LogInModel logInModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await userService.Login(logInModel.Id, logInModel.Password))
                    {
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    return View();
                }
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

        //LOGOUT
        [Authorize]
        [HttpGet]
        public IActionResult LogOut()
        {
            try
            {
                userService.LogOut();

                return RedirectToAction("Login");

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

        //GET ERROR VIEW
        [HttpGet]
        public IActionResult Error()
        {
            try
            {
                return View("ErrorView", "Er is ergens iets mis gegaan");
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