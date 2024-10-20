using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repos
{
    public class StudentRepo : IStudentRepo
    {
        private FoodWasteContext ctx;

        public StudentRepo(FoodWasteContext ctx)
        {
            this.ctx = ctx;
        }

        public IEnumerable<Student?> GetStudents() => ctx.Students.ToList();

        public Student? GetStudent(int studentNr) 
            => ctx.Students.Where(s => s.StudentNr == studentNr).Include(s => s.Pakkages).FirstOrDefault();

        public void ReservePakkage(Pakkage pakkage)
        {
            ctx.Update(pakkage);
            ctx.SaveChanges();
        }
    }
}
