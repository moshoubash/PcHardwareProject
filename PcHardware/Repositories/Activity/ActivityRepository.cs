

using PcHardware.Services;

namespace PcHardware.Repositories.Activity
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly MyDbContext dbContext;
        public ActivityRepository(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        void IActivityRepository.DeleteActivity(int Id)
        {
            dbContext.Activities.Remove(dbContext.Activities.FirstOrDefault(a => a.Id == Id));
            dbContext.SaveChanges();
        }

        List<Models.Activity> IActivityRepository.GetActivities()
        {
            return dbContext.Activities.ToList();
        }
    }
}
