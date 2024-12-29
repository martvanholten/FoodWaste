using NSubstitute.ReturnsExtensions;

namespace ApplicationServicesTests
{
    public class PakkageServiceTests
    {
        private IPakkageService pakkageService;
        private IPakkageRepo pakkageRepo = Substitute.For<IPakkageRepo>();
        private IProductRepo productRepo = Substitute.For<IProductRepo>();

        public PakkageServiceTests()
        {
            this.pakkageService = new PakkageService(this.pakkageRepo, this.productRepo);
        }

        //GET
        [Fact]
        public void GetPakkage_ShouldReturnPakkage_WhenItExists()
        {
            //Arrange
            Pakkage actualPakkage = new Pakkage
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
                }
            };

            pakkageRepo.GetPakkage(actualPakkage.Title).Returns(actualPakkage);
            //Act
            Pakkage? pakkage = pakkageRepo.GetPakkage(actualPakkage.Title);
            //Assert
            Assert.Equal(actualPakkage.Title, pakkage?.Title);
        }

        [Fact]
        public void GetPakkage_ShouldFail_WhenItDoesNotExist()
        {
            //Arrange
            pakkageRepo.GetPakkage(Arg.Any<string>()).ReturnsNull();
            //Act
            ErrorModel ex = (ErrorModel)Record.Exception(()
            => pakkageService.GetPakkage("Sandwich"));
            //Assert
            Assert.Equal($"Het pakket Sandwich bestaat niet", ex.Error);
        }

        //ADD
        [Fact]
        public void AddPakkage_ShouldSucceed_WhenItDoesNotExist()
        {
            //Arrange
            Pakkage newPakkage = new Pakkage
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
                }
            };

            pakkageRepo.GetPakkage(Arg.Any<string>()).ReturnsNull();
            //Act
            pakkageService.AddPakkage(newPakkage);
            //Assert
        }

        [Fact]
        public void AddPakkage_ShouldFail_WhenItExists()
        {
            //Arrange
            Pakkage newPakkage = new Pakkage
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
                }
            };

            Pakkage oldPakkage = new Pakkage
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
                }
            };

            pakkageRepo.GetPakkage(newPakkage.Title).Returns(oldPakkage);
            //Act
            ErrorModel ex = (ErrorModel)Record.Exception(()
            => pakkageService.AddPakkage(newPakkage));
            //Assert
            Assert.Equal($"Het pakket Sandwich bestaat al", ex.Error);
        }

        //REMOVE
        [Fact]
        public void RemovePakkage_ShouldSucceed_WhenItExists()
        {
            //Arrange
            Pakkage pakkage = new Pakkage
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
                }
            };

            pakkageRepo.GetPakkage(pakkage.Title).Returns(pakkage);
            //Act
            pakkageService.RemovePakkage(pakkage.Title);
            //Assert
        }

        [Fact]
        public void RemovePakkage_ShouldFail_WhenItDoesNotExist()
        {
            //Arrange
            pakkageRepo.GetPakkage(Arg.Any<string>()).ReturnsNull();
            //Act
            ErrorModel ex = (ErrorModel)Record.Exception(()
            => pakkageService.RemovePakkage("Sandwich"));
            //Assert
            Assert.Equal($"Het pakket Sandwich bestaat niet", ex.Error);
        }

        [Fact]
        public void RemovePakkage_ShouldFail_WhenItIsReserved()
        {
            //Arrange
            Pakkage pakkage = new Pakkage
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

            pakkageRepo.GetPakkage(pakkage.Title).Returns(pakkage);
            //Act
            ErrorModel ex = (ErrorModel)Record.Exception(()
            => pakkageService.RemovePakkage(pakkage.Title));
            //Assert
            Assert.Equal($"Het pakket {pakkage.Title} is gereserveerd", ex.Error);
        }

        //UPDATE
        [Fact]
        public void UpdatePakkage_ShouldSucceed_WhenItIsNotReserved()
        {
            //Arrange
            Pakkage oldPakkage = new Pakkage
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
            };

            Pakkage pakkage = new Pakkage
            {
                Title = "Sandwich",
                City = "Breda",
                Cantine = "LD",
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
            };

            pakkageRepo.GetPakkage(oldPakkage.Title).Returns(oldPakkage);
            //Act
            pakkageService.UpdatePakkage(pakkage);
            //Assert
        }

        [Fact]
        public void UpdatePakkage_ShouldFail_WhenItIsReserved()
        {
            //Arrange
            Pakkage oldPakkage = new Pakkage
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

            Pakkage pakkage = new Pakkage
            {
                Title = "Sandwich",
                City = "Breda",
                Cantine = "LD",
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

            pakkageRepo.GetPakkage(oldPakkage.Title).Returns(oldPakkage);
            //Act
            ErrorModel ex = (ErrorModel)Record.Exception(()
            => pakkageService.UpdatePakkage(pakkage));
            //Assert
            Assert.Equal($"Het pakket {oldPakkage.Title} is gereserveerd", ex.Error);
        }

        //ADD PRODUCT
        [Fact]
        public void AddProduct_ShouldSucceed_WhenPakkageDoesNotHaveProduct()
        {
            //Arrange
            Pakkage pakkage = new Pakkage
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
            };

            Product product = new Product
            {
                Title = "Tomaat",
                Photo = "Foto",
                Alchol = 0
            };

            pakkageRepo.GetPakkage(pakkage.Title).Returns(pakkage);
            productRepo.GetProduct(product.Title).Returns(product);
            //Act
            pakkageService.AddProduct(pakkage.Title, product.Title);
            //Assert
        }

        [Fact]
        public void AddProduct_ShouldFail_WhenPakkageDoesHaveProduct()
        {
            //Arrange
            Pakkage pakkage = new Pakkage
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
            };

            Product product = new Product
            {
                Title = "Tomaat",
                Photo = "Foto",
                Alchol = 0
            };

            pakkage.Products.Add(product);

            pakkageRepo.GetPakkage(pakkage.Title).Returns(pakkage);
            productRepo.GetProduct(product.Title).Returns(product);
            //Act
            ErrorModel ex = (ErrorModel)Record.Exception(()
            => pakkageService.AddProduct(pakkage.Title, product.Title));
            //Assert
            Assert.Equal($"Het pakket {pakkage.Title} heeft het product al", ex.Error);
        }

        [Fact]
        public void AddProduct_ShouldFail_WhenPakkageDoesNotExist()
        {
            //Arrange
            Product product = new Product
            {
                Title = "Tomaat",
                Photo = "Foto",
                Alchol = 0
            };

            pakkageRepo.GetPakkage(Arg.Any<string>()).ReturnsNull();
            productRepo.GetProduct(product.Title).Returns(product);
            //Act
            ErrorModel ex = (ErrorModel)Record.Exception(()
            => pakkageService.AddProduct("Sandwich", product.Title));
            //Assert
            Assert.Equal($"Het pakket Sandwich bestaat niet", ex.Error);
        }

        [Fact]
        public void AddProduct_ShouldFail_WhenProductDoesNotExist()
        {
            //Arrange
            Pakkage pakkage = new Pakkage
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
            };

            pakkageRepo.GetPakkage(pakkage.Title).Returns(pakkage);
            productRepo.GetProduct(Arg.Any<string>()).ReturnsNull();
            //Act
            ErrorModel ex = (ErrorModel)Record.Exception(()
            => pakkageService.AddProduct(pakkage.Title, "Tomaat"));
            //Assert
            Assert.Equal($"Het product Tomaat bestaat niet", ex.Error);
        }

        [Fact]
        public void AddProduct_ShouldFail_WhenPakkageIsReserved()
        {
            //Arrange
            Pakkage pakkage = new Pakkage
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

            Product product = new Product
            {
                Title = "Tomaat",
                Photo = "Foto",
                Alchol = 0
            };

            pakkageRepo.GetPakkage(pakkage.Title).Returns(pakkage);
            productRepo.GetProduct(product.Title).Returns(product);
            //Act
            ErrorModel ex = (ErrorModel)Record.Exception(()
            => pakkageService.AddProduct(pakkage.Title, product.Title));
            //Assert
            Assert.Equal($"Het pakket {pakkage.Title} is gereserveerd", ex.Error);
        }

        [Fact]
        public void AddProduct_ShouldChangeAgeRestriction_WhenProductHasAgeRestriction()
        {
            //Arrange
            Pakkage pakkage = new Pakkage
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
            };

            Product product = new Product
            {
                Title = "Bier",
                Photo = "Foto",
                Alchol = 1
            };

            pakkageRepo.GetPakkage(pakkage.Title).Returns(pakkage);
            productRepo.GetProduct(product.Title).Returns(product);
            //Act
            pakkageService.AddProduct(pakkage.Title, product.Title);
            //Assert
            Assert.Equal(1, pakkage.AgeRestriction);
        }

        //REMOVE PRODUCT
        [Fact]
        public void RemoveProduct_ShouldSucceed_WhenPakkageDoesHaveProduct()
        {
            //Arrange
            Pakkage pakkage = new Pakkage
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
            };

            Product product = new Product
            {
                Title = "Tomaat",
                Photo = "Foto",
                Alchol = 0
            };

            pakkage.Products.Add(product);

            pakkageRepo.GetPakkage(pakkage.Title).Returns(pakkage);
            productRepo.GetProduct(product.Title).Returns(product);
            //Act
            pakkageService.RemoveProduct(pakkage.Title, product.Title);
            //Assert
        }

        [Fact]
        public void RemoveProduct_ShouldFail_WhenPakkageDoesNotHaveProduct()
        {
            //Arrange
            Pakkage pakkage = new Pakkage
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
            };

            Product product = new Product
            {
                Title = "Tomaat",
                Photo = "Foto",
                Alchol = 0
            };

            pakkageRepo.GetPakkage(pakkage.Title).Returns(pakkage);
            productRepo.GetProduct(product.Title).Returns(product);
            //Act
            ErrorModel ex = (ErrorModel)Record.Exception(()
            => pakkageService.RemoveProduct(pakkage.Title, product.Title));
            //Assert
            Assert.Equal($"Het pakket {pakkage.Title} heeft het product niet", ex.Error);
        }

        [Fact]
        public void RemoveProduct_ShouldFail_WhenPakkageDoesNotExist()
        {
            //Arrange
            Product product = new Product
            {
                Title = "Tomaat",
                Photo = "Foto",
                Alchol = 0
            };

            pakkageRepo.GetPakkage(Arg.Any<string>()).ReturnsNull();
            productRepo.GetProduct(product.Title).Returns(product);
            //Act
            ErrorModel ex = (ErrorModel)Record.Exception(()
            => pakkageService.RemoveProduct("Sandwich", product.Title));
            //Assert
            Assert.Equal($"Het pakket Sandwich bestaat niet", ex.Error);
        }

        [Fact]
        public void RemoveProduct_ShouldFail_WhenProductDoesNotExist()
        {
            //Arrange
            Pakkage pakkage = new Pakkage
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
            };

            pakkageRepo.GetPakkage(pakkage.Title).Returns(pakkage);
            productRepo.GetProduct(Arg.Any<string>()).ReturnsNull();
            //Act
            ErrorModel ex = (ErrorModel)Record.Exception(()
            => pakkageService.RemoveProduct(pakkage.Title, "Tomaat"));
            //Assert
            Assert.Equal($"Het product Tomaat bestaan niet", ex.Error);
        }

        [Fact]
        public void RemoveProduct_ShouldFail_WhenPakkageIsReserved()
        {
            //Arrange
            Pakkage pakkage = new Pakkage
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

            Product product = new Product
            {
                Title = "Tomaat",
                Photo = "Foto",
                Alchol = 0
            };

            pakkage.Products.Add(product);

            pakkageRepo.GetPakkage(pakkage.Title).Returns(pakkage);
            productRepo.GetProduct(product.Title).Returns(product);
            //Act
            ErrorModel ex = (ErrorModel)Record.Exception(()
            => pakkageService.RemoveProduct(pakkage.Title, product.Title));
            //Assert
            Assert.Equal($"Het pakket {pakkage.Title} is gereserveerd", ex.Error);
        }

        [Fact]
        public void RemoveProduct_ShouldChangeAgeRestriction_WhenProductHasAgeRestriction()
        {
            //Arrange
            Pakkage pakkage = new Pakkage
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
            };

            Product product = new Product
            {
                Title = "Bier",
                Photo = "Foto",
                Alchol = 1
            };

            pakkage.Products.Add(product);

            pakkageRepo.GetPakkage(pakkage.Title).Returns(pakkage);
            productRepo.GetProduct(product.Title).Returns(product);
            //Act
            pakkageService.RemoveProduct(pakkage.Title, product.Title);
            //Assert
            Assert.Equal(0, pakkage.AgeRestriction);
        }
    }
}