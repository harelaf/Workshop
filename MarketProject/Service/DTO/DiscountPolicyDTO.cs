namespace MarketProject.Service.DTO
{
    public class DiscountPolicyDTO
    {
        public PlusDiscountDTO discounts;

        public DiscountPolicyDTO(PlusDiscountDTO discounts)
        {
            this.discounts = discounts;
        }
    }
}
