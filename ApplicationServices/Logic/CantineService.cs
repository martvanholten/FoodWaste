namespace ApplicationServices.Logic
{
    public class CantineService : ICantineService
    {
        private ICantineRepo cantineRepo;

        public CantineService(ICantineRepo cantineRepo)
        {
            this.cantineRepo = cantineRepo;
        }
        public IEnumerable<Cantine?> GetCantines()
        {
            try
            {
                return cantineRepo.GetCantines();
            }
            catch
            {
                throw new ErrorModel($"Database fout", 500);
            }
        }
        public IEnumerable<IGrouping<string?, Cantine?>> GetCitys()
        {
            try
            {
                return cantineRepo.GetCitys();
            }
            catch
            {
                throw new ErrorModel($"Database fout", 500);
            }
        }

        public IEnumerable<Cantine?> GetCantinesByCity(string city)
        {
            try
            {
                return cantineRepo.GetCantinesByCity(city);
            }
            catch
            {
                throw new ErrorModel($"Database fout", 500);
            }
        }

        public Cantine GetCantine(string city, string location)
        {
            try
            {
                Cantine? cantine = cantineRepo.GetCantine(city, location);
                if (cantine == null)
                {
                    throw new ErrorModel($"De cantine {location} in {city} bestaat niet", 404);
                }
                else
                {
                    return cantine;
                }
            }
            catch (Exception ex)
            {
                if (ex is ErrorModel)
                {
                    ErrorModel er = (ErrorModel)ex;
                    throw er;
                }
                throw new ErrorModel($"Database fout", 500);
            }
        }
    }
}