namespace ApplicationServices.Logic
{
    public class StudentService : IStudentService
    {
        private IStudentRepo studentRepo;
        private IPakkageRepo pakkageRepo;

        public StudentService(IStudentRepo studentRepo, IPakkageRepo pakkageRepo)
        {
            this.studentRepo = studentRepo;
            this.pakkageRepo = pakkageRepo;
        }

        public IEnumerable<Student?> GetStudents() 
        {
            try
            {
                return studentRepo.GetStudents();
            }
            catch
            {
                throw new ErrorModel("Database fout", 500);
            }
        }

        public void ReservePakkage(string title, string studentNr)
        {
            try
            {
                Student? student = studentRepo.GetStudent(int.Parse(studentNr));
                Pakkage? pakkage = pakkageRepo.GetPakkage(title);
                bool freeDate = true;

                if (student == null)
                {
                    throw new ErrorModel($"Student met het nummer {studentNr} bestaat niet", 404);
                }

                if (pakkage == null)
                {
                    throw new ErrorModel($"Het pakket {title} bestaat niet", 404);
                }

                var year = pakkage.PickUpDate.Year - student.DateOfBirth.Year - 1;
                var month = pakkage.PickUpDate.Month - student.DateOfBirth.Month;
                var day = pakkage.PickUpDate.Day - student.DateOfBirth.Day;
                if (month < 0 || (month == 0 && day < 1))
                {
                    year += 1;
                }

                foreach (Pakkage? p in student!.Pakkages)
                {
                    if (p.PickUpDate == pakkage?.PickUpDate)
                    {
                        freeDate = false;
                        break;
                    }
                }

                if (pakkage?.AgeRestriction == 1 && year < 18)
                {
                    throw new ErrorModel("Je moet 18+ zijn", 409);
                }
                else if (pakkage?.ReservedFor != null)
                {
                    throw new ErrorModel($"Het pakket {title} is al gereserveerd", 404);
                }
                else if (!freeDate)
                {
                    throw new ErrorModel("Je kan maar 1 pakket per dag ophalen", 409);
                }
                else
                {
                    pakkage!.ReservedFor = student.StudentNr;

                    studentRepo.ReservePakkage(pakkage);
                }
            }
            catch (Exception ex)
            {
                if (ex is ErrorModel)
                {
                    ErrorModel er = (ErrorModel)ex;
                    throw er;
                }
                throw new ErrorModel("Database fout", 500);
            }
        }
    }
}
