namespace PcHardware.Repositories.Warehouse
{
    public interface IWarehouseRepository
    {
        List<Models.Warehouse> GetWarehouses();
        Models.Warehouse GetWarehouse(int Id);
        void CreateWarehouse(Models.Warehouse warehouse);
        void EditWarehouse(Models.Warehouse warehouse);
        void DeleteWarehouse(int Id);
    }
}
