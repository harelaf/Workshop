using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Server.DataLayer
{
    public class DiscountDAL
    {
        [Key]
        public int _id { get; set; }
        public ConditionDAL _condition { get; set; }
        public DiscountDAL(ConditionDAL condition)
        {
            _condition = condition;
        }

        public DiscountDAL()
        {
            // ???
        }
    }
    public class AtomicDiscountDAL : DiscountDAL
    {
        [Required]
        public DateTime _expiration { get; set; }

        protected AtomicDiscountDAL(DateTime expiration, ConditionDAL condition) : base(condition)
        {
        }

        protected AtomicDiscountDAL()
        {
            // ???
        }
    }
    public class PercentageDiscountDAL : AtomicDiscountDAL
    {
        [Required]
        public double _percents { get; set; }
        [Required]
        public DateTime _expiration { get; set; }

        protected PercentageDiscountDAL(double percents, DateTime expiration, ConditionDAL condition) : base(expiration, condition)
        {
            _percents = percents;
            _expiration = expiration;
        }

        protected PercentageDiscountDAL()
        {
            // ???
        }
    }
    public class AllProductsDiscountDAL : PercentageDiscountDAL
    {
        public AllProductsDiscountDAL(double percentage_to_subtract, DateTime expiration, ConditionDAL condition) : base(percentage_to_subtract, expiration, condition)
        {
        }

        public AllProductsDiscountDAL()
        {
            // ???
        }
    }
    public class CategoryDiscountDAL : PercentageDiscountDAL
    {
        [Required]
        public string _categoryName { get; set; }
        public CategoryDiscountDAL(string categoryName, double percents, DateTime expiration, ConditionDAL condition) : base(percents, expiration, condition)
        {
            _categoryName = categoryName;
        }

        public CategoryDiscountDAL()
        {
            // ???
        }
    }
    public class ItemDiscountDAL : PercentageDiscountDAL
    {
        [Required]
        public string _itemName { get; set; }
        public ItemDiscountDAL(string itemName, double percents, DateTime expiration, ConditionDAL condition) : base(percents, expiration, condition)
        {
            _itemName = itemName;
        }

        public ItemDiscountDAL()
        {
            // ???
        }
    }
    public class NumericDiscountDAL : AtomicDiscountDAL
    {
        [Required]
        public double _priceToSubtract { get; set; }
        public NumericDiscountDAL(double priceToSubtract, DateTime expiration, ConditionDAL condition) : base(expiration, condition)
        {
            _priceToSubtract = priceToSubtract;
        }
        public NumericDiscountDAL()
        {
            // ???
        }
    }
    public class ComposedDiscountDAL : DiscountDAL
    {
        public List<DiscountDAL> _discounts { get; set; }
        protected ComposedDiscountDAL(List<DiscountDAL> discounts, ConditionDAL condition) : base(condition)
        {
            _discounts = discounts;
        }
        protected ComposedDiscountDAL()
        {
            // ???
        }
    }
    public class MaxDiscountDAL : ComposedDiscountDAL
    {
        public MaxDiscountDAL(List<DiscountDAL> discounts, ConditionDAL condition) : base(discounts, condition)
        {
        }
        public MaxDiscountDAL()
        {
            // ???
        }
    }
    public class PlusDiscountDAL : ComposedDiscountDAL
    {
        public PlusDiscountDAL(List<DiscountDAL> discounts, ConditionDAL condition) : base(discounts, condition)
        {
        }
        public PlusDiscountDAL()
        {
            // ???
        }
    }
}