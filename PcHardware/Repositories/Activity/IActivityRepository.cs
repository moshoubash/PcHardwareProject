namespace PcHardware.Repositories.Activity
{
    public interface IActivityRepository
    {
        List<Models.Activity> GetActivities();
        void DeleteActivity(int Id);
    }
}
