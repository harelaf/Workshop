using MarketWeb.Shared.DTO;
using System;

namespace MarketWeb.Client.Helpers
{
    public class StringFormatter
    {
        public static String discountDetailsToString(DiscountDetailsDTO details)
        {
            if (details == null)
                return "null";
            if (details.DiscountList == null)
                return "other null";
            String str = "";
            foreach (AtomicDiscountDTO dis in details.DiscountList)
                str += $"{discountToString(dis)}\n";
            return str;
        }

        public static String discountToString(AtomicDiscountDTO dis)
        {
            if (dis == null)
                return "";
            Type type = dis.GetType();
            if (type.Equals(typeof(AllProductsDiscountDTO)))
                return specificDiscountToString((AllProductsDiscountDTO)dis);
            if (type.Equals(typeof(CategoryDiscountDTO)))
                return specificDiscountToString((CategoryDiscountDTO)dis);
            if (type.Equals(typeof(ItemDiscountDTO)))
                return specificDiscountToString((ItemDiscountDTO)dis);
            if (type.Equals(typeof(NumericDiscountDTO)))
                return specificDiscountToString((NumericDiscountDTO)dis);
            else throw new NotImplementedException($"need an implementation for {type} discount type.");
        }

        public static String conditionToString(IConditionDTO cond)
        {
            if (cond == null)
                return "";
            Type type = cond.GetType();
            if (type.Equals(typeof(AndCompositionDTO)))
                return specificConditionToString((AndCompositionDTO)cond);
            if (type.Equals(typeof(DayOnWeekConditionDTO)))
                return specificConditionToString((DayOnWeekConditionDTO)cond);
            if (type.Equals(typeof(HourConditionDTO)))
                return specificConditionToString((HourConditionDTO)cond);
            if (type.Equals(typeof(OrCompositionDTO)))
                return specificConditionToString((OrCompositionDTO)cond);
            if (type.Equals(typeof(PriceableConditionDTO)))
                return specificConditionToString((PriceableConditionDTO)cond);
            if (type.Equals(typeof(SearchCategoryConditionDTO)))
                return specificConditionToString((SearchCategoryConditionDTO)cond);
            if (type.Equals(typeof(SearchItemConditionDTO)))
                return specificConditionToString((SearchItemConditionDTO)cond);
            if (type.Equals(typeof(XorCompositionDTO)))
                return specificConditionToString((XorCompositionDTO)cond);
            else throw new NotImplementedException();
        }

        public static String specificConditionToString(AndCompositionDTO condition)
        {
            String str = "Apply AND logic over the following condition(s):\n";
            int index = 1;
            foreach (IConditionDTO innerCond in condition.Conditions)
                str += $"{index++}. {conditionToString(innerCond)}\n";
            return str;
        }

        public static String specificConditionToString(OrCompositionDTO condition)
        {
            String str = "Apply OR logic over the following condition(s):\n";
            int index = 1;
            foreach (IConditionDTO innerCond in condition.Conditions)
                str += $"{index++}. {conditionToString(innerCond)}\n";
            return str;
        }

        public static String specificConditionToString(XorCompositionDTO condition)
        {
            String str = "Apply XOR logic over the following condition(s):\n";
            int index = 1;
            foreach (IConditionDTO innerCond in condition.Conditions)
                str += $"{index++}. {conditionToString(innerCond)}\n";
            return str;
        }

        public static String specificConditionToString(SearchItemConditionDTO condition)
        {
            String neg = condition.Negative ? "NOT " : "";
            String maxStr = condition.MaxAmount > condition.MinAmount ? $" and under {condition.MaxAmount}" : "";
            return $"the basket is {neg}containing over {condition.MinAmount}{maxStr} of '{condition.KeyWord}'";
        }

        public static String specificConditionToString(SearchCategoryConditionDTO condition)
        {
            String neg = condition.Negative ? "NOT " : "";
            String maxStr = condition.MaxAmount > condition.MinAmount ? $" and under {condition.MaxAmount}" : "";
            return $"the basket is {neg}containing over {condition.MinAmount}{maxStr} of '{condition.KeyWord}' category";
        }

        public static String specificConditionToString(DayOnWeekConditionDTO condition)
        {
            String neg = condition.Negative ? "NOT " : "";
            return $"today is {condition.DayOnWeek}";
        }

        public static String specificConditionToString(HourConditionDTO condition)
        {
            String neg = condition.Negative ? "NOT " : "";
            return $"it is {neg}between {condition.MinHour} and {condition.MaxHour} o'clock";
        }

        public static String specificConditionToString(PriceableConditionDTO condition)
        {
            String neg = condition.Negative ? "NOT " : "";
            String maxStr = condition.MaxAmount > condition.MinAmount ? $" and under {condition.MaxAmount}" : "";
            return $"the total price is {neg}over {condition.MinAmount}{maxStr}";
        }

        public static String specificDiscountToString(AllProductsDiscountDTO dis)
        {
            return $"{dis.Percentage}% off all products\nexpired on: {dis.Expiration}\ncondition: {conditionToString(dis.Condition)}";
        }

        public static String specificDiscountToString(ItemDiscountDTO dis)
        {
            return $"{dis.PercentageToSubtract}% off '{dis.ItemName}' products\nexpired on: {dis.Expiration}\ncondition: {conditionToString(dis.Condition)}";
        }

        public static String specificDiscountToString(CategoryDiscountDTO dis)
        {
            return $"{dis.Percentage_to_subtract}% off '{dis.Category}' category products\nexpired on: {dis.Expiration}\ncondition: {conditionToString(dis.Condition)}";
        }

        public static String specificDiscountToString(NumericDiscountDTO dis)
        {
            return $"{dis.PriceToSubtract}% off the total price\nexpired on: {dis.Expiration}\ncondition: {conditionToString(dis.Condition)}";
        }
    }
}
