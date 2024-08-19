namespace PcHardware.Repositories.Manufacturer
{
    public interface IManufacturerRepository
    {
        public void CreateManufacturer(Models.Manufacturer manufacturer);
        public void DeleteManufacturer(int Id);
        public List<Models.Manufacturer> GetManufacturers();
        public Task<Models.Manufacturer> GetManufacturerProductsAsync(int Id);
    }
}
