using System;

namespace MarketProject.Domain.PurchasePackage.PolicyPackage
{
    public class ItemDiscount : PercentageDiscount
    {
        private String _itemName;
        public String ItemName
        {
            get { return _itemName; }
            private set { _itemName = value; }
        }
        public ItemDiscount(double percentage_to_subtract, String itemName, Condition _condition, DateTime expiration) : base(percentage_to_subtract, _condition, expiration)
        {
            _itemName = itemName;
        }

        public ItemDiscount(double percentage_to_subtract, String itemName, DateTime expiration) : base(percentage_to_subtract, expiration)
        {
            _itemName = itemName;
        }

        public override void applyDiscount(ISearchablePriceable searchablePriceable)
        {
            if (!CheckCondition(searchablePriceable) || GetExpirationDate(searchablePriceable) < DateTime.Now)
                return;
            searchablePriceable.SetItemDiscount(this, ItemName);
        }

        public override string GetDiscountString(int indent)
        {
            return PercentageToSubtract + "% off " + _itemName + "." +
                ExpirationToString(indent) + 
                ConditionToString(indent);
        }

        public override string GetActualDiscountString(ISearchablePriceable searchablePriceable, int indent)
        {
            return PercentageToSubtract + "% off " + _itemName + "." +
                $"{newLine(indent)}{searchablePriceable.GetItemPrice(ItemName)} - {GetTotalDiscount(searchablePriceable)} = {searchablePriceable.GetItemPrice(ItemName) - GetTotalDiscount(searchablePriceable)}" + 
                ExpirationToString(indent) +
                ConditionToString(indent);
        }
        public override double GetTotalDiscount(ISearchablePriceable searchablePriceable)
        {
            if (!CheckCondition(searchablePriceable) || GetExpirationDate(searchablePriceable) < DateTime.Now)
                return 0;
            return searchablePriceable.GetItemPrice(ItemName) * PercentageToSubtract / 100;
        }
    }
}
