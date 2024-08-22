namespace PcHardware.Repositories.Metatag
{
    public interface IMetatagRepository
    {
        List<Models.MetaTag> GetMetaTags();
        Models.MetaTag GetMetaTagById(int Id);
        void CreateMeta(Models.MetaTag metatag);
        void EditMeta(Models.MetaTag metatag);
        void DeleteMeta(int Id);
    }
}
