using System;
using System.Collections.Generic;
using System.Text;
using MarketProject.Domain;
using MarketProject.Domain.PurchasePackage.DiscountPackage;
using MarketProject.Service.DTO;

namespace MarketProject.Service
{
    public class DTOtranslator
    {
        public DiscountCondition translateCondition(IConditionDTO cond)
        {
            Type type = cond.GetType();
            if(type.Equals(typeof(AndCompositionDTO)))
                return translate((AndCompositionDTO)cond);
            if (type.Equals(typeof(DayOnWeekConditionDTO)))
                return translate((DayOnWeekConditionDTO)cond);
            if (type.Equals(typeof(HourConditionDTO)))
                return translate((HourConditionDTO)cond);
            if (type.Equals(typeof(OrCompositionDTO)))
                return translate((OrCompositionDTO)cond);
            if (type.Equals(typeof(PriceableConditionDTO)))
                return translate((PriceableConditionDTO)cond);
            if (type.Equals(typeof(SearchCategoryConditionDTO)))
                return translate((SearchCategoryConditionDTO)cond);
            if (type.Equals(typeof(SearchItemConditionDTO)))
                return translate((SearchItemConditionDTO)cond);
            if (type.Equals(typeof(XorCompositionDTO)))
                return translate((XorCompositionDTO)cond);
            else throw new NotImplementedException();
        }
        public Discount translateDiscount(IDiscountDTO dis)
        {
            Type type = dis.GetType();
            if (type.Equals(typeof(AllProductsDiscountDTO)))
                return translate((AllProductsDiscountDTO)dis);
            if (type.Equals(typeof(CategoryDiscountDTO)))
                return translate((CategoryDiscountDTO)dis);
            if (type.Equals(typeof(ItemDiscountDTO)))
                return translate((ItemDiscountDTO)dis);
            if (type.Equals(typeof(MaxDiscountDTO)))
                return translate((MaxDiscountDTO)dis);
            if (type.Equals(typeof(NumericDiscountDTO)))
                return translate((NumericDiscountDTO)dis);
            if (type.Equals(typeof(PlusDiscountDTO)))
                return translate((PlusDiscountDTO)dis);
            else throw new NotImplementedException($"need an implementation for {type} discount type.");
        }
        public AndComposition translate(AndCompositionDTO condition_dto)
        {
            bool negative = condition_dto.Negative;
            List<DiscountCondition> conditions = new List<DiscountCondition>();
            foreach (IConditionDTO cond in condition_dto.Conditions)
                conditions.Add(translateCondition(cond));
            return new AndComposition(negative, conditions);
        }
        public DayOnWeekCondition translate(DayOnWeekConditionDTO condition_dto)
        {
            String day = condition_dto.DayOnWeek;
            bool negative = condition_dto.Negative;
            return new DayOnWeekCondition(day, negative);
        }
        public DiscountCondition translate(HourConditionDTO condition_dto)
        {
            int minHour = condition_dto.MinHour;
            int maxHour = condition_dto.MaxHour;
            bool negative = condition_dto.Negative;
            return new HourCondition(minHour, maxHour, negative);
        }
        public DiscountCondition translate(OrCompositionDTO condition_dto)
        {
            bool negative = condition_dto.Negative;
            List<DiscountCondition> conditions = new List<DiscountCondition>();
            foreach (IConditionDTO cond in condition_dto.Conditions)
                conditions.Add(translateCondition(cond));
            return new OrComposition(negative, conditions);
        }
        public DiscountCondition translate(PriceableConditionDTO condition_dto)
        {
            String keyWord = condition_dto.KeyWord;
            int minAmount = condition_dto.MinAmount;
            int maxAmount = condition_dto.MaxAmount;
            bool negative = condition_dto.Negative;
            return new PriceableCondition(keyWord, minAmount, maxAmount, negative);
        }
        public DiscountCondition translate(SearchCategoryConditionDTO condition_dto)
        {
            String keyWord = condition_dto.KeyWord;
            int minAmount = condition_dto.MinAmount;
            int maxAmount = condition_dto.MaxAmount;
            bool negative = condition_dto.Negative;
            return new SearchCategoryCondition(keyWord, minAmount, maxAmount, negative);
        }
        public DiscountCondition translate(SearchItemConditionDTO condition_dto)
        {
            String keyWord = condition_dto.KeyWord;
            int minAmount = condition_dto.MinAmount;
            int maxAmount = condition_dto.MaxAmount;
            bool negative = condition_dto.Negative;
            return new SearchItemCondition(keyWord, minAmount, maxAmount, negative);
        }
        public DiscountCondition translate(XorCompositionDTO condition_dto)
        {
            bool negative = condition_dto.Negative;
            List<DiscountCondition> conditions = new List<DiscountCondition>();
            foreach (IConditionDTO cond in condition_dto.Conditions)
                conditions.Add(translateCondition(cond));
            return new XorComposition(negative, conditions);
        }
        public Discount translate(AllProductsDiscountDTO discount_dto)
        {
            double percentage = discount_dto.Percentage;
            DiscountCondition condition = translateCondition(discount_dto.Condition);
            DateTime expiration = discount_dto.Expiration;
            return new AllProductsDiscount(percentage, condition, expiration);
        }
        public Discount translate(CategoryDiscountDTO discount_dto)
        {
            double percentage_to_subtract = discount_dto.Percentage_to_subtract;
            String category = discount_dto.Category;
            DiscountCondition condition = translateCondition(discount_dto.Condition);
            DateTime expiration = discount_dto.Expiration;
            return new CategoryDiscount(percentage_to_subtract, category, condition, expiration);
        }
        public Discount translate(ItemDiscountDTO discount_dto)
        {
            double percentage_to_subtract = discount_dto.PercentageToSubtract;
            String category = discount_dto.ItemName;
            DiscountCondition condition = translateCondition(discount_dto.Condition);
            DateTime expiration = discount_dto.Expiration;
            return new ItemDiscount(percentage_to_subtract, category, condition, expiration);
        }
        public Discount translate(MaxDiscountDTO discount_dto)
        {
            List<Discount> discount_list = new List<Discount>();
            foreach (IDiscountDTO discountDTO in discount_dto.Discounts)
                discount_list.Add(translateDiscount(discountDTO));
            DiscountCondition condition = translateCondition(discount_dto.Condition);
            return new MaxDiscount(discount_list, condition);
        }
        public Discount translate(NumericDiscountDTO discount_dto)
        {
            double priceToSubtract = discount_dto.PriceToSubtract;
            DiscountCondition condition = translateCondition(discount_dto.Condition);
            DateTime expiration = discount_dto.Expiration;
            return new NumericDiscount(priceToSubtract, condition, expiration);
        }
        public Discount translate(PlusDiscountDTO discount_dto)
        {
            List<Discount> discounts = new List<Discount>();
            foreach (IDiscountDTO discountDTO in discount_dto.Discounts)
                discounts.Add(translateDiscount(discountDTO));
            DiscountCondition condition = translateCondition(discount_dto.Condition);
            return new PlusDiscount(discounts, condition);
        }
    }
}
