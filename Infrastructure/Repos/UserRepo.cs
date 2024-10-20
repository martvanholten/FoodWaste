using Infrastructure.Data;

namespace Infrastructure.Repos
{
    public class UserRepo : IUserRepo
    {
        private FoodWasteContext ctx;

        public UserRepo(FoodWasteContext ctx)
        {
            this.ctx = ctx;
        }

        public string GetUserRole(string id) 
        {
            var user = ctx.UserRoles.Where(u => u.UserId.Equals(id)).FirstOrDefault();
            var role = ctx.Roles.Where(r => r.Id.Equals(user!.RoleId)).FirstOrDefault();

            return role!.Name!;
        }
    }
}
