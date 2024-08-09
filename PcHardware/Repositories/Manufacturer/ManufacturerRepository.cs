
using PcHardware.Services;

namespace PcHardware.Repositories.Manufacturer
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly MyDbContext dbContext;
        public ManufacturerRepository(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        void IManufacturerRepository.CreateManufacturer(Models.Manufacturer manufacturer)
        {
            dbContext.Manufactureres.Add(manufacturer);
            dbContext.SaveChanges();
        }

        void IManufacturerRepository.DeleteManufacturer(int Id)
        {
            var targetManu = dbContext.Manufactureres.Where(m => m.Id == Id).FirstOrDefault();
            dbContext.Manufactureres.Remove(targetManu);
            dbContext.SaveChanges();
        }

        List<Models.Manufacturer> IManufacturerRepository.GetManufacturers()
        {
            return dbContext.Manufactureres.ToList();
        }
    }
}
