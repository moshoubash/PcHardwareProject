
using PcHardware.Services;
using System.Linq;

namespace PcHardware.Repositories.Warehouse
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly MyDbContext dbContext;
        public WarehouseRepository(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        void IWarehouseRepository.CreateWarehouse(Models.Warehouse warehouse)
        {
            dbContext.Warehouses.Add(warehouse);
            dbContext.SaveChanges();
        }

        void IWarehouseRepository.DeleteWarehouse(int Id)
        {
            var targetWarehouse = dbContext.Warehouses.FirstOrDefault(w => w.Id == Id);
            if (targetWarehouse != null)
            {
                dbContext.Warehouses.Remove(targetWarehouse);
                dbContext.SaveChanges();
            }
        }

        void IWarehouseRepository.EditWarehouse(Models.Warehouse warehouse)
        {
            var targetWarehouse = dbContext.Warehouses.FirstOrDefault(w => w.Id == warehouse.Id);
            if (targetWarehouse != null)
            {
                dbContext.Warehouses.Update(targetWarehouse);
            }
        }

        Models.Warehouse IWarehouseRepository.GetWarehouse(int Id)
        {
            return dbContext.Warehouses.FirstOrDefault(w => w.Id == Id);
        }

        List<Models.Warehouse> IWarehouseRepository.GetWarehouses()
        {
            return dbContext.Warehouses.ToList();
        }
    }
}
