using System.Linq;

namespace ApplicationServices.Logic
{
    public class PakkageService : IPakkageService
    {
        private IPakkageRepo pakkageRepo;
        private IProductRepo productRepo;
        private ICantineService cantineService;

        public PakkageService(IPakkageRepo pakkageRepo, IProductRepo productRepo, ICantineService cantineService)
        {
            this.pakkageRepo = pakkageRepo;
            this.productRepo = productRepo;
            this.cantineService = cantineService;
        }

        public void AddPakkage(Pakkage pakkage)
        {
            try
            {
                if (pakkageRepo.GetPakkage(pakkage.Title) != null)
                {
                    throw new ErrorModel($"Het pakket {pakkage.Title} bestaat al", 409);
                }
                else
                {
                    pakkageRepo.AddPakkage(pakkage);
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

        public Pakkage GetPakkage(string title)
        {
            try
            {
                Pakkage? pakkage = pakkageRepo.GetPakkage(title);
                if (pakkage == null)
                {
                    throw new ErrorModel($"Het pakket {title} bestaat niet", 404);
                }
                else
                {
                    return pakkage;
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

        public IEnumerable<Pakkage?> GetPakkagesFromCantine(string city, string cantine)
        {
            try
            {
                return pakkageRepo.GetPakkagesFromCantine(city, cantine);
            }
            catch
            {
                throw new ErrorModel("Database fout", 500);
            }
        }

        public IEnumerable<Pakkage?> GetPakkages()
        {
            try
            {
                return pakkageRepo.GetPakkages();
            }
            catch
            {
                throw new ErrorModel("Database fout", 500);
            }
        }

        public IEnumerable<Pakkage?> GetPakkagesByType(string city, string cantine, string type)
        {
            try
            {
                return pakkageRepo.GetPakkagesByType(city, cantine, type);
            }
            catch
            {
                throw new ErrorModel("Database fout", 500);
            }
        }

        public IEnumerable<Pakkage?> GetPakkagesForStudent(int studentNr)
        {
            try
            {
                return pakkageRepo.GetPakkagesForStudent(studentNr);
            }
            catch
            {
                throw new ErrorModel("Database fout", 500);
            }
        }

        public IEnumerable<Pakkage?> GetPakkagesReservedByStudent(int studentNr)
        {
            try
            {
                return pakkageRepo.GetPakkagesReservedByStudent(studentNr);
            }
            catch
            {
                throw new ErrorModel("Database fout", 500);
            }
        }

        public void RemovePakkage(string title)
        {
            try
            {
                Pakkage? pakkage = pakkageRepo.GetPakkage(title);
                if (pakkage != null)
                {
                    if (pakkage.ReservedFor != null)
                    {
                        throw new ErrorModel($"Het pakket {title} is gereserveerd", 409);
                    }
                    else
                    {
                        pakkageRepo.RemovePakkage(pakkage);
                    }
                }
                else
                {
                    throw new ErrorModel($"Het pakket {title} bestaat niet", 404);
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

        public void UpdatePakkage(Pakkage pakkage)
        {
            try
            {
                if (pakkage.ReservedFor != null)
                {
                    throw new ErrorModel($"Het pakket {pakkage.Title} is gereserveerd", 409);
                }
                else
                {
                    pakkageRepo.UpdatePakkage(pakkage);
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

        public void AddProduct(string pakkageTitle, string productTitle)
        {
            try
            {
                Pakkage? pakkage = pakkageRepo.GetPakkage(pakkageTitle);
                if (pakkage != null)
                {
                    if (pakkage.ReservedFor != null)
                    {
                        throw new ErrorModel($"Het pakket {pakkage.Title} is gereserveerd", 409);
                    }
                    Product? product = productRepo.GetProduct(productTitle);

                    if (product != null)
                    {
                        if (!pakkage.Products.Contains(product))
                        {
                            pakkage.Products.Add(product);

                            if (product.Alchol == 1)
                            {
                                pakkage.AgeRestriction = 1;
                            }

                            pakkageRepo.UpdatePakkage(pakkage);
                        }
                        else
                        {
                            throw new ErrorModel($"Het pakket {pakkageTitle} heeft het product al", 404);
                        }
                    }
                    else
                    {
                        throw new ErrorModel($"Het product {productTitle} bestaat niet", 404);
                    }
                }
                else
                {
                    throw new ErrorModel($"Het pakket {pakkageTitle} bestaat niet", 404);
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

        public void RemoveProduct(string pakkageTitle, string productTitle)
        {
            try
            {
                Pakkage? pakkage = pakkageRepo.GetPakkage(pakkageTitle);
                if (pakkage != null)
                {
                    if (pakkage.ReservedFor != null)
                    {
                        throw new ErrorModel($"Het pakket {pakkageTitle} is gereserveerd", 409);
                    }
                    Product? product = productRepo.GetProduct(productTitle);

                    if (product != null)
                    {
                        if (pakkage.Products.Contains(product))
                        {
                            pakkage.Products.Remove(product);

                            int age = 0;

                            foreach (var p in pakkage.Products)
                            {
                                if (p.Alchol == 1)
                                {
                                    age = 1;
                                    break;
                                }
                            }

                            pakkage.AgeRestriction = age;

                            pakkageRepo.UpdatePakkage(pakkage);
                        }
                        else
                        {
                            throw new ErrorModel($"Het pakket {pakkageTitle} heeft het product niet", 404);
                        }
                    }
                    else
                    {
                        throw new ErrorModel($"Het product {productTitle} bestaan niet", 404);
                    }
                }
                else
                {
                    throw new ErrorModel($"Het pakket {pakkageTitle} bestaat niet", 404);
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
