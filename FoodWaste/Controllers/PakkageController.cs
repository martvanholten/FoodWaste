using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;

namespace UserInterface.Controllers
{
    public class PakkageController : Controller
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
        public IActionResult All()
        {
            try
            {
                CantinePakkages viewModel = new CantinePakkages()
                {
                    City = cantineService.GetCitys(),
                };

                if (userService.GetUserRole(User.Identity.GetUserId()).Equals("Employ"))
                {
                    return View("PakkagesEmploy", viewModel);
                }
                else
                {
                    return View("PakkagesStudent", new StudentNrPakkages
                    {
                        StudentNr = int.Parse(User.Identity.GetUserId()),
                        CantinePakkages = viewModel
                    });
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

        //GET PAKKAGES RESERVED BY STUDENT
        [Authorize(Roles = "Student")]
        [HttpGet]
        public IActionResult Student(int studentNr)
        {
            try
            {
                CantinePakkages cantinePakkages = new CantinePakkages()
                {
                    Pakkages = pakkageService.GetPakkagesReservedByStudent(studentNr),
                };

                StudentNrPakkages viewModel = new StudentNrPakkages
                {
                    StudentNr = studentNr,
                    CantinePakkages = cantinePakkages
                };

                return View("PakkagesStudent", viewModel);

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

        //GET ONE PAKKAGE
        [HttpGet]
        [Authorize(Roles="Employ, Student")]
        public IActionResult Pakkage(string title)
        {
            try
            {
                Pakkage pakkage = pakkageService.GetPakkage(title);

                if (userService.GetUserRole(User.Identity.GetUserId()).Equals("Employ"))
                {
                    ViewUpdatePakkage viewPakkage = new ViewUpdatePakkage
                    {
                        Title = pakkage.Title,
                        City = pakkage.City,
                        Cantine = pakkage.Cantine,
                        ExperationDate = pakkage.ExperationDate,
                        PickUpDate = pakkage.PickUpDate,
                        AgeRestriction = pakkage.AgeRestriction,
                        Type = pakkage.Type,
                        Price = pakkage.Price,
                        ReservedFor = pakkage.ReservedFor,
                        Products = pakkage.Products,
                    };
                    return View("PakkageEmploy", viewPakkage);
                    }
                else
                {
                    return View("PakkageStudent", pakkage);
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

        //UPDATE ONE PAKKAGE
        [HttpPost]
        [Authorize(Roles="Employ")]
        public IActionResult Update(ViewUpdatePakkage viewPakkage, string oldTitle)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Pakkage pakkage = pakkageService.GetPakkage(oldTitle);
                    Cantine cantine = cantineService.GetCantine
                            (viewPakkage.City ?? pakkage.City, viewPakkage.Cantine ?? pakkage.Cantine);
                    pakkage.Title = pakkage.Title;
                    pakkage.City = viewPakkage.City ?? pakkage.City;
                    pakkage.Cantine = viewPakkage.Cantine ?? pakkage.Cantine;
                    pakkage.PickUpDate = viewPakkage.PickUpDate ?? pakkage.PickUpDate;
                    pakkage.ExperationDate = viewPakkage.ExperationDate ?? pakkage.ExperationDate;
                    pakkage.AgeRestriction = viewPakkage.AgeRestriction ?? pakkage.AgeRestriction;
                    pakkage.Price = viewPakkage.Price ?? pakkage.Price;
                    if (viewPakkage.Type == null)
                    {
                        pakkage.Type = pakkage.Type;
                    } else if (viewPakkage.Type.Equals("Ontbijt"))
                    {
                        pakkage.Type = "Breakfast";
                    }else if (viewPakkage.Type.Equals("Lunch"))
                    {
                        pakkage.Type = "Lunch";
                    }else if (viewPakkage.Type.Equals("Aavond maal"))
                    {
                        pakkage.Type = "Evning meal";
                    }
                    pakkage.CantineNavigation = cantine;

                    pakkageService.UpdatePakkage(pakkage);

                    return View("PakkageEmploy", viewPakkage);
                }
                else
                {
                    return View("PakkageEmploy", viewPakkage);
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

        //DELETE ONE PAKKAGE
        [HttpGet]
        [Authorize(Roles = "Employ")]
        public IActionResult Delete(string title)
        {
            try
            {
                pakkageService.RemovePakkage(title);
                CantinePakkages viewModel = new CantinePakkages()
                {
                    City = cantineService.GetCitys(),
                };

                return View("PakkagesEmploy", viewModel);
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

        //GET ADD ONE PAKKAGE VIEW
        [HttpGet]
        [Authorize(Roles = "Employ")]
        public IActionResult Add()
        {
            try
            {
                return View("PakkageAdd");
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

        //ADD ONE PAKKAGE
        [HttpPost]
        [Authorize(Roles="Employ")]
        public IActionResult Add(ViewAddPakkage viewPakkage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Cantine cantine = cantineService.GetCantine(viewPakkage.City, viewPakkage.Cantine);

                    Pakkage pakkage = new Pakkage()
                    {
                        Title = viewPakkage.Title,
                        City = viewPakkage.City,
                        Cantine = viewPakkage.Cantine,
                        PickUpDate = viewPakkage.PickUpDate,
                        ExperationDate = viewPakkage.ExperationDate,
                        AgeRestriction = 0,
                        Price = viewPakkage.Price,
                        Type = viewPakkage.Type
                    };

                    if (viewPakkage.Type.Equals("Ontbijt"))
                    {
                        pakkage.Type = "Breakfast";
                    }
                    else if (viewPakkage.Type.Equals("Lunch"))
                    {
                        pakkage.Type = "Lunch";
                    }
                    else if (viewPakkage.Type.Equals("Aavond maal"))
                    {
                        pakkage.Type = "Evning meal";
                    }

                    pakkageService.AddPakkage(pakkage);

                    CantinePakkages viewModel = new CantinePakkages()
                    {
                        City = cantineService.GetCitys(),
                    };

                    return View("PakkagesEmploy", viewModel);
                }
                return View("PakkageAdd", viewPakkage);
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

        //ADD ONE PRODUCT TO PAKKAGE
        [HttpPost]
        [Authorize(Roles = "Employ")]
        public IActionResult AddProduct(string pakkageTitle, string productTitle)
        {
            try
            {
                pakkageService.AddProduct(pakkageTitle, productTitle);
                Pakkage pakkage = pakkageService.GetPakkage(pakkageTitle);
                ViewUpdatePakkage viewPakkage = new ViewUpdatePakkage
                {
                    Title = pakkage.Title,
                    City = pakkage.City,
                    Cantine = pakkage.Cantine,
                    ExperationDate = pakkage.ExperationDate,
                    PickUpDate = pakkage.PickUpDate,
                    AgeRestriction = pakkage.AgeRestriction,
                    Type = pakkage.Type,
                    Price = pakkage.Price,
                    ReservedFor = pakkage.ReservedFor,
                    Products = pakkage.Products,
                };
                return View("PakkageEmploy", viewPakkage);
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

        //DELETE ONE PRODUCT FROM PAKKAGE
        [HttpGet]
        [Authorize(Roles="Employ")]
        public IActionResult DeleteProduct(string pakkageTitle, string productTitle)
        {
            try
            {
                pakkageService.RemoveProduct(pakkageTitle, productTitle);
                Pakkage pakkage = pakkageService.GetPakkage(pakkageTitle);
                ViewUpdatePakkage viewPakkage = new ViewUpdatePakkage
                {
                    Title = pakkage.Title,
                    City = pakkage.City,
                    Cantine = pakkage.Cantine,
                    ExperationDate = pakkage.ExperationDate,
                    PickUpDate = pakkage.PickUpDate,
                    AgeRestriction = pakkage.AgeRestriction,
                    Type = pakkage.Type,
                    Price = pakkage.Price,
                    ReservedFor = pakkage.ReservedFor,
                    Products = pakkage.Products,
                };
                return View("PakkageEmploy", viewPakkage);
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

        //RESERVE A PAKKAGE
        [HttpGet]
        [Authorize(Roles="Student")]
        public IActionResult Reserve(string title)
        {
            try
            {
                studentService.ReservePakkage(title, User.Identity.GetUserId());
                return View("PakkageStudent", pakkageService.GetPakkage(title));
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

        //GET ALL CANTINES FROM A CITY
        [HttpGet]
        [Authorize(Roles = "Employ, Student")]
        public IActionResult CantinesByCity(string city)
        {
            try
            {
                CantinePakkages viewModel = new CantinePakkages()
                {
                    Cantine = cantineService.GetCantinesByCity(city),
                    City = cantineService.GetCitys(),
                };

                if (userService.GetUserRole(User.Identity.GetUserId()).Equals("Employ"))
                {
                    return View("PakkagesEmploy", viewModel);
                }
                else
                {
                    return View("PakkagesStudent", new StudentNrPakkages
                    {
                        StudentNr = int.Parse(User.Identity.GetUserId()),
                        CantinePakkages = viewModel
                    });
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

        //GET PAKKAGES FROM CANTINE
        [HttpGet]
        [Authorize(Roles="Employ, Student")]
        public IActionResult CantinePakkages(string city, string cantine)
        {
            try
            {
                CantinePakkages viewModel = new CantinePakkages()
                {
                    Cantine = cantineService.GetCantinesByCity(city),
                    City = cantineService.GetCitys(),
                    Pakkages = pakkageService.GetPakkagesFromCantine(city, cantine)
                };

                if (userService.GetUserRole(User.Identity.GetUserId()).Equals("Employ"))
                {
                    return View("PakkagesEmploy", viewModel);
                }
                else
                {
                    return View("PakkagesStudent", new StudentNrPakkages
                    {
                        StudentNr = int.Parse(User.Identity.GetUserId()),
                        CantinePakkages = viewModel
                    });
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

        //GET PAKKAGES FROM CANTINE FILTERED BY TYPE
        [HttpGet]
        [Authorize(Roles="Employ, Student")]
        public IActionResult Filtered(string city, string cantine, string type)
        {
            try
            {
                CantinePakkages viewModel = new CantinePakkages()
                {
                    Cantine = cantineService.GetCantinesByCity(city),
                    City = cantineService.GetCitys(),
                    Pakkages = pakkageService.GetPakkagesByType(city, cantine, type),
                };

                if (userService.GetUserRole(User.Identity.GetUserId()).Equals("Employ"))
                {
                    return View("PakkagesEmploy", viewModel);
                }
                else
                {
                    return View("PakkagesStudent", new StudentNrPakkages
                {
                    StudentNr = int.Parse(User.Identity.GetUserId()),
                    CantinePakkages = viewModel
                    });
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
    }
}