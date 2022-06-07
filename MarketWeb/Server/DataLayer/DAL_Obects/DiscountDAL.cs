using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Server.DataLayer
{
    internal class DiscountDAL
    {
        internal ConditionDAL _condition;
        internal DiscountDAL(ConditionDAL condition)
        {
            _condition = condition;
        }
    }
    internal abstract class AtomicDiscountDAL : DiscountDAL
    {
        [Required]
        internal DateTime _expiration;

        internal AtomicDiscountDAL(DateTime expiration, ConditionDAL condition) : base(condition)
        {
        }
    }
    internal abstract class PercentageDiscountDAL : AtomicDiscountDAL
    {
        [Required]
        internal double _percents;
        [Required]
        internal DateTime _expiration;

        internal PercentageDiscountDAL(int percents, DateTime expiration, ConditionDAL condition) : base(expiration, condition)
        {
            _percents = percents;
            _expiration = expiration;
        }
    }
    internal class AllProductsDiscountDAL : PercentageDiscountDAL
    {
        internal AllProductsDiscountDAL(double percentage_to_subtract, DateTime expiration, ConditionDAL condition) : base(percentage_to_subtract, expiration, condition)
        {
        }
    }
    internal class CategoryDiscountDAL : PercentageDiscountDAL
    {
        [Required]
        internal string _categoryName;
        internal CategoryDiscountDAL(string categoryName, double percents, DateTime expiration, ConditionDAL condition) : base(percents, expiration, condition)
        {
            _categoryName = categoryName;
        }
    }
    internal class ItemDiscountDAL : PercentageDiscountDAL
    {
        [Required]
        internal string _itemName;
        internal ItemDiscountDAL(string itemName, double percents, DateTime expiration, ConditionDAL condition) : base(percents, expiration, condition)
        {
            _itemName = itemName;
        }
    }
    internal class NumericDiscountDAL : AtomicDiscountDAL
    {
        [Required]
        internal double _priceToSubtract;
        internal NumericDiscountDAL(double priceToSubtract, DateTime expiration, ConditionDAL condition) : base(expiration, condition)
        {
            _priceToSubtract = priceToSubtract;
        }
    }
    internal abstract class ComposedDiscountDAL : DiscountDAL
    {
        internal List<DiscountDAL> _discounts;
        internal ComposedDiscountDAL(List<DiscountDAL> discounts, ConditionDAL condition) : base(condition)
        {
            _discounts = discounts;
        }
    }
    internal class MaxDiscountDAL : ComposedDiscountDAL
    {
        internal MaxDiscountDAL(List<DiscountDAL> discounts, ConditionDAL condition) : base(discounts, condition)
        {
        }
    }
    internal class PlusDiscountDAL : ComposedDiscountDAL
    {
        internal PlusDiscountDAL(List<DiscountDAL> discounts, ConditionDAL condition) : base(discounts, condition)
        {
        }
    }
}