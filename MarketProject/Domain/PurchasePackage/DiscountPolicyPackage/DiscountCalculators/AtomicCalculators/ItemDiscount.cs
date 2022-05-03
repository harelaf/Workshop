using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    public class ItemDiscount : PercentageDiscount
    {
        private String _itemName;
        public String ItemName
        {
            get { return _itemName; }
            private set { _itemName = value; }
        }
        public ItemDiscount(double percentage_to_subtract, String itemName, DiscountCondition _condition, DateTime expiration) : base(percentage_to_subtract, _condition, expiration)
        {
            _itemName = itemName;
        }

        public ItemDiscount(double percentage_to_subtract, String itemName, DateTime expiration) : base(percentage_to_subtract, expiration)
        {
            _itemName = itemName;
        }

        public override double GetTotalDiscount(ISearchablePriceable searchablePriceable)
        {
            if (!CheckCondition(searchablePriceable) || GetExpirationDate(searchablePriceable) < DateTime.Now)
                return 0;
            return searchablePriceable.GetItemPrice(_itemName) * PercentageToSubtract / 100;
        }

        public override string GetDiscountString(ISearchablePriceable searchablePriceable)
        {
            return PercentageToSubtract + "% off " + _itemName + "." +
                "\n\n" + ExpirationToString() + 
                "\n\n" + ConditionToString();
        }
    }
}
