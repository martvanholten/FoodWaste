namespace ApplicationServices.Logic
{
    public class EmployService : IEmployService
    {
        private IEmployRepo employRepo;

        public EmployService(IEmployRepo employRepo)
        {
            this.employRepo = employRepo;
        }

        public IEnumerable<Employ?> GetEmploys()
        {
            try
            {
                return employRepo.GetEmploys();
            }
            catch
            {
                throw new ErrorModel("Database fout", 500);
            }
        }
    }
}
