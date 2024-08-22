using Microsoft.EntityFrameworkCore;
using PcHardware.Models;
using PcHardware.Services;

namespace PcHardware.Repositories.Metatag
{
    public class MetatagRepository : IMetatagRepository
    {
        private readonly MyDbContext dbContext;
        public MetatagRepository(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        void IMetatagRepository.CreateMeta(MetaTag metatag)
        {
            dbContext.MetaTags.Add(metatag);
            dbContext.SaveChanges();
        }

        void IMetatagRepository.DeleteMeta(int Id)
        {
            var targetMeta = (from m in dbContext.MetaTags where m.Id == Id select m).FirstOrDefault();
            dbContext.MetaTags.Remove(targetMeta);
            dbContext.SaveChanges();
        }

        void IMetatagRepository.EditMeta(MetaTag metatag)
        {
            dbContext.MetaTags.Update(metatag);
            dbContext.SaveChanges();
        }

        MetaTag IMetatagRepository.GetMetaTagById(int Id)
        {
            return dbContext.MetaTags.FirstOrDefault(m => m.Id == Id);
        }

        List<MetaTag> IMetatagRepository.GetMetaTags()
        {
            return dbContext.MetaTags.ToList();
        }
    }
}
