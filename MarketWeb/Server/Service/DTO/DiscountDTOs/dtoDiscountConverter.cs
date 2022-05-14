using MarketProject.Domain.PurchasePackage.DiscountPackage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Service.DTO
{
    public class dtoDiscountConverter
    {
        dtoConditionConverter conCon = new dtoConditionConverter();
        public Discount convertDiscount(IDiscountDTO discount_dto)
        {
            return discount_dto.ConvertMe(this);
        }

        //////////  add all convertion methods for each discount  /////////////
        public Discount ConvertConcrete(AllProductsDiscountDTO discount_dto)
        {
            double percentage = discount_dto.Percentage;
            DiscountCondition condition = conCon.convertCondition(discount_dto.Condition);
            DateTime expiration = discount_dto.Expiration;
            return new AllProductsDiscount(percentage, condition, expiration);
        }
        public Discount ConvertConcrete(CategoryDiscountDTO discount_dto)
        {
            double percentage_to_subtract = discount_dto.Percentage_to_subtract;
            String category = discount_dto.Category;
            DiscountCondition condition = conCon.convertCondition(discount_dto.Condition);
            DateTime expiration = discount_dto.Expiration;
            return new CategoryDiscount(percentage_to_subtract, category, condition, expiration);
        }
        public Discount ConvertConcrete(ItemDiscountDTO discount_dto)
        {
            double percentage_to_subtract = discount_dto.PercentageToSubtract;
            String category = discount_dto.ItemName;
            DiscountCondition condition = conCon.convertCondition(discount_dto.Condition);
            DateTime expiration = discount_dto.Expiration;
            return new ItemDiscount(percentage_to_subtract, category, condition, expiration);
        }
        public Discount ConvertConcrete(MaxDiscountDTO discount_dto)
        {
            List<Discount> discount_list = new List<Discount>();
            foreach(IDiscountDTO discountDTO in discount_dto.Discounts)
                discount_list.Add(convertDiscount(discountDTO));
            DiscountCondition condition = conCon.convertCondition(discount_dto.Condition);
            return new MaxDiscount(discount_list, condition);
        }
        public Discount ConvertConcrete(NumericDiscountDTO discount_dto)
        {
            double priceToSubtract = discount_dto.PriceToSubtract;
            DiscountCondition condition = conCon.convertCondition(discount_dto.Condition);
            DateTime expiration = discount_dto.Expiration;
            return new NumericDiscount(priceToSubtract, condition, expiration);
        }
        public Discount ConvertConcrete(PlusDiscountDTO discount_dto)
        {
            List<Discount> discounts = new List<Discount>();
            foreach(IDiscountDTO discountDTO in discount_dto.Discounts)
                discounts.Add(convertDiscount(discountDTO));
            DiscountCondition condition = conCon.convertCondition(discount_dto.Condition);
            return new PlusDiscount(discounts, condition);
        }
    }
}
