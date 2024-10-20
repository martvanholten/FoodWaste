namespace ApplicationServices.Interfaces
{
    public interface IStudentService
    {
        public IEnumerable<Student?> GetStudents();

        public void ReservePakkage(string title, string studentNr);
    }
}
