using Domain.Models;
using NSubstitute.ReturnsExtensions;

namespace ApplicationServicesTests
{
    public class StudentServiceTest
    {
        private IStudentService studentService;
        private IPakkageRepo pakkageRepo = Substitute.For<IPakkageRepo>();
        private IStudentRepo studentRepo = Substitute.For<IStudentRepo>();

        public StudentServiceTest()
        {
            this.studentService = new StudentService(this.studentRepo, this.pakkageRepo);
        }

        [Theory]
        [MemberData(nameof(reservationAgeSucces))]
        public void ReservePakkage_AgeRestriction_ShouldSucceed_WhenStudentIsOfAge(Student student)
        {
            //Arrange
            Pakkage pakkage = new Pakkage
            {
                Title = "Bier",
                City = "Breda",
                Cantine = "LA",
                PickUpDate = new DateTime(2018, 09, 18),
                ExperationDate = new DateTime(2018, 09, 20),
                AgeRestriction = 1,
                Price = 2,
                Type = "Lunch",
                CantineNavigation = new Cantine
                {
                    City = "Breda",
                    Location = "LA",
                    Warm = 1
                }
            };

            studentRepo.GetStudent(student.StudentNr).Returns(student);
            pakkageRepo.GetPakkage(pakkage.Title).Returns(pakkage);
            //Act
            studentService.ReservePakkage(pakkage.Title, student.StudentNr.ToString());
            //Assert
        }

        [Theory]
        [MemberData(nameof(reservationAgeFail))]
        public void ReservePakkage_AgeRestriction_ShouldFail_WhenStudentIsNotOfAge(Student student)
        {
            //Arrange
            Pakkage pakkage = new Pakkage
            {
                Title = "Bier",
                City = "Breda",
                Cantine = "LA",
                PickUpDate = new DateTime(2018, 09, 18),
                ExperationDate = new DateTime(2018, 09, 20),
                AgeRestriction = 1,
                Price = 2,
                Type = "Lunch",
                CantineNavigation = new Cantine
                {
                    City = "Breda",
                    Location = "LA",
                    Warm = 1
                }
            };

            studentRepo.GetStudent(student.StudentNr).Returns(student);
            pakkageRepo.GetPakkage(pakkage.Title).Returns(pakkage);
            //Act
            ErrorModel ex = (ErrorModel)Record.Exception(()
                => studentService.ReservePakkage(pakkage.Title, student.StudentNr.ToString()));
            //Assert
            Assert.Equal("Je moet 18+ zijn", ex.Error);
        }

        [Fact]
        public void ReservePakkage_Reserved_ShouldFail_WhenPakkageIsAlreadyReserved()
        {
            //Arrange
            Student student = new Student
            {
                StudentNr = 1,
                Name = "Mart",
                City = "Breda",
                DateOfBirth = new DateOnly(2000, 09, 18),
                Email = "martvanholten@hotmail.com",
                Phonenumber = 0636457987
            };

            Pakkage pakkage = new Pakkage
            {
                Title = "Bier",
                City = "Breda",
                Cantine = "LA",
                PickUpDate = new DateTime(2018, 09, 18),
                ExperationDate = new DateTime(2018, 09, 20),
                AgeRestriction = 1,
                Price = 2,
                Type = "Lunch",
                CantineNavigation = new Cantine
                {
                    City = "Breda",
                    Location = "LA",
                    Warm = 1
                },
                ReservedFor = 5
            };

            studentRepo.GetStudent(student.StudentNr).Returns(student);
            pakkageRepo.GetPakkage(pakkage.Title).Returns(pakkage);
            //Act
            ErrorModel ex = (ErrorModel)Record.Exception(()
                => studentService.ReservePakkage(pakkage.Title, student.StudentNr.ToString()));
            //Assert
            Assert.Equal($"Het pakket {pakkage.Title} is al gereserveerd", ex.Error);
        }

        [Fact]
        public void ReservePakkage_Reserved_ShouldFail_WhenPakkageDoesNotExist()
        {
            //Arrange
            Student student = new Student
            {
                StudentNr = 1,
                Name = "Mart",
                City = "Breda",
                DateOfBirth = new DateOnly(2000, 09, 18),
                Email = "martvanholten@hotmail.com",
                Phonenumber = 0636457987
            };

            studentRepo.GetStudent(student.StudentNr).Returns(student);
            pakkageRepo.GetPakkage(Arg.Any<string>()).ReturnsNull();
            //Act
            ErrorModel ex = (ErrorModel)Record.Exception(()
                => studentService.ReservePakkage("Sandwich", student.StudentNr.ToString()));
            //Assert
            Assert.Equal($"Het pakket Sandwich bestaat niet", ex.Error);
        }

        [Fact]
        public void ReservePakkage_Reserved_ShouldFail_WhenStudentDoesNotExist()
        {
            //Arrange
            Pakkage pakkage = new Pakkage
            {
                Title = "Bier",
                City = "Breda",
                Cantine = "LA",
                PickUpDate = new DateTime(2018, 09, 18),
                ExperationDate = new DateTime(2018, 09, 20),
                AgeRestriction = 1,
                Price = 2,
                Type = "Lunch",
                CantineNavigation = new Cantine
                {
                    City = "Breda",
                    Location = "LA",
                    Warm = 1
                },
            };

            studentRepo.GetStudent(Arg.Any<int>()).ReturnsNull();
            pakkageRepo.GetPakkage(pakkage.Title).Returns(pakkage);
            //Act
            ErrorModel ex = (ErrorModel)Record.Exception(()
                => studentService.ReservePakkage(pakkage.Title, "1"));
            //Assert
            Assert.Equal($"Student met het nummer 1 bestaat niet", ex.Error);
        }

        [Fact]
        public void ReservePakkage_Reserved_ShouldFail_WhenStudentHasAPakkageToPickUpThatDay()
        {
            //Arrange
            Pakkage pakkage = new Pakkage
            {
                Title = "Bier",
                City = "Breda",
                Cantine = "LA",
                PickUpDate = new DateTime(2018, 09, 18),
                ExperationDate = new DateTime(2018, 09, 20),
                AgeRestriction = 1,
                Price = 2,
                Type = "Lunch",
                CantineNavigation = new Cantine
                {
                    City = "Breda",
                    Location = "LA",
                    Warm = 1
                },
            };
            Pakkage reservedPakkage = new Pakkage
            {
                Title = "Sandwich",
                City = "Breda",
                Cantine = "LA",
                PickUpDate = new DateTime(2018, 09, 18),
                ExperationDate = new DateTime(2018, 09, 20),
                AgeRestriction = 0,
                Price = 2,
                Type = "Lunch",
                CantineNavigation = new Cantine
                {
                    City = "Breda",
                    Location = "LA",
                    Warm = 1
                },
                ReservedFor = 1
            };
            Student student = new Student
            {
                StudentNr = 1,
                Name = "Mart",
                City = "Breda",
                DateOfBirth = new DateOnly(2000, 09, 18),
                Email = "martvanholten@hotmail.com",
                Phonenumber = 0636457987
            };
            student.Pakkages.Add(reservedPakkage);

            studentRepo.GetStudent(student.StudentNr).Returns(student);
            pakkageRepo.GetPakkage(pakkage.Title).Returns(pakkage);
            //Act
            ErrorModel ex = (ErrorModel)Record.Exception(()
                => studentService.ReservePakkage(pakkage.Title, student.StudentNr.ToString()));
            //Assert
            Assert.Equal("Je kan maar 1 pakket per dag ophalen", ex.Error);
        }

        public static IEnumerable<object[]> reservationAgeSucces
            => new[]
            {
                new object[] {
                    new Student
                    {
                        StudentNr = 1,
                        Name = "Mart",
                        City = "Breda",
                        DateOfBirth = new DateOnly(2000, 09, 18),
                        Email = "martvanholten@hotmail.com",
                        Phonenumber = 0636457987
                    },
                },
                new object[] {
                    new Student
                    {
                        StudentNr = 1,
                        Name = "Mart",
                        City = "Breda",
                        DateOfBirth = new DateOnly(2000, 10, 18),
                        Email = "martvanholten@hotmail.com",
                        Phonenumber = 0636457987,
                    },
                },
                new object[] {
                    new Student
                    {
                        StudentNr = 1,
                        Name = "Mart",
                        City = "Breda",
                        DateOfBirth = new DateOnly(2000, 09, 19),
                        Email = "martvanholten@hotmail.com",
                        Phonenumber = 0636457987
                    },
                }
            };

        public static IEnumerable<object[]> reservationAgeFail
            => new[]
            {
                new object[] {
                    new Student
                    {
                        StudentNr = 1,
                        Name = "Mart",
                        City = "Breda",
                        DateOfBirth = new DateOnly(2000, 09, 17),
                        Email = "martvanholten@hotmail.com",
                        Phonenumber = 0636457987
                    },
                },
                new object[] {
                    new Student
                    {
                        StudentNr = 1,
                        Name = "Mart",
                        City = "Breda",
                        DateOfBirth = new DateOnly(2000, 08, 18),
                        Email = "martvanholten@hotmail.com",
                        Phonenumber = 0636457987
                    },
                }
            };
    }
}