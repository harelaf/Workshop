using MarketProject.Domain.PurchasePackage.DiscountPackage;

namespace MarketProject.Service.DTO
{
    public interface IConditionDTO
    {
        public DiscountCondition ConvertMe(dtoConditionConverter converter);
    }
}