using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    internal class ItemDiscount : PercentageDiscount
    {
        private Item _item;
        public Item Item
        {
            get { return _item; }
            private set { _item = value; }
        }
        public ItemDiscount(double percentage_to_subtract, Item item, DiscountCondition _condition, DateTime expiration) : base(percentage_to_subtract, _condition, expiration)
        {
            _item = item;
        }

        public ItemDiscount(double percentage_to_subtract, Item item, DateTime expiration) : base(percentage_to_subtract, expiration)
        {
            _item = item;
        }

        public override double GetTotalDiscount(ShoppingCart cart)
        {
            if (!CheckCondition() || GetExpirationDate(cart) < DateTime.Now)
                return 0;
            return cart.GetTotalPrice(_item) * PercentageToSubtract / 100;
        }

        public override string GetDiscountString(ShoppingCart cart)
        {
            return PercentageToSubtract + "% off " + _item.Name + "." +
                "\n\n" + ExpirationToString() + 
                "\n\n" + ConditionToString();
        }
    }
}
