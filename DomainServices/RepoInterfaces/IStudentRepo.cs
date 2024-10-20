namespace DomainServices.RepoInterfaces
{
    public interface IStudentRepo
    {
        public IEnumerable<Student?> GetStudents();

        public Student? GetStudent(int studentNr);

        public void ReservePakkage(Pakkage pakkage);
    }
}
