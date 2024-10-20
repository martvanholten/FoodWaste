namespace ApplicationServices.Interfaces
{
    public interface IUserService
    {
        public Task<string?> Register(string name, string id, string password, string email, string role);

        public Task<bool> VerifyEmail(string email, string password, string code);

        public Task<bool> Login(string email, string password);

        public void LogOut();

        public string GetUserRole(string id);
    }
}
