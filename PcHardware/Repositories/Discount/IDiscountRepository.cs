namespace PcHardware.Repositories.Discount
{
    public interface IDiscountRepository
    {
        List<Models.Discount> GetDiscounts();
        Models.Discount GetDiscountById(int Id);
        
        void CreateDiscount(Models.Discount discount);
        void DeleteDiscount(int Id);
        void EditDiscount(int Id, Models.Discount discount);
    }
}
