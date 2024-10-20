using Microsoft.AspNetCore.Identity;

namespace ApplicationServices.Logic
{
    public class UserService : IUserService
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private IStudentService studentService;
        private IEmployService employService;
        private IUserRepo userRepo;

        public UserService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, 
            IUserRepo userRepo, IStudentService studentService, IEmployService employService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.userRepo = userRepo;
            this.studentService = studentService;
            this.employService = employService;
        }

        public async Task<string?> Register(string name, string id, string password, string email, string role)
        {
            try
            {
                IEnumerable<Student?> students = studentService.GetStudents();
                IEnumerable<Employ?> employs = employService.GetEmploys();

                if (students.Where(s => s?.StudentNr == int.Parse(id)) == null
                    && employs.Where(e => e?.EmployNr == int.Parse(id)) == null)
                {
                    throw new ErrorModel("No student or emply with this number exists", 404);
                }

                var user = new IdentityUser
                {
                    UserName = name,
                    Id = id,
                    Email = email
                };

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);

                    return await userManager.GenerateEmailConfirmationTokenAsync(user);
                }

                return null;
            }
            catch (Exception ex)
            {
                if (ex is ErrorModel)
                {
                    ErrorModel er = (ErrorModel)ex;
                    throw er;
                }
                throw new ErrorModel($"Er is iets fout gegaan", 400);
            }
        }

        public async Task<bool> VerifyEmail(string email, string password, string code)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    return false;
                }
                var result = await userManager.ConfirmEmailAsync(user, code);

                var signInResult = await signInManager.PasswordSignInAsync(user, password, false, false);

                if (signInResult.Succeeded)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                throw new ErrorModel($"Er is iets fout gegaan", 400);
            }
        }

        public async Task<bool> Login(string id, string password)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id);

                if (user != null)
                {
                    var signInResult = await signInManager.PasswordSignInAsync(user, password, false, false);

                    if (signInResult.Succeeded)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch
            {
                throw new ErrorModel($"Er is iets fout gegaan", 400);
            }
        }

        public async void LogOut() 
        {
            try
            {
                await signInManager.SignOutAsync();
            }
            catch
            {
                throw new ErrorModel($"Er is iets fout gegaan", 400);
            }
        }

        public string GetUserRole(string id) 
        {
            try
            {
                return userRepo.GetUserRole(id);
            }
            catch
            {
                throw new ErrorModel($"Er is iets fout gegaan", 400);
            }
        } 
    }
}
