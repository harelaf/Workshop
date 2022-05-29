using MarketWeb.Server.Domain.PurchasePackage.DiscountPackage;
using System;
using System.Collections.Generic;

namespace MarketWeb.Server.Domain.PurchasePackage.DiscountPolicyPackage
{
    public class DiscountParser
    {
        private String DiscountString;
        private String ConditionString;
        private int ConditionIndex = 0;
        private bool NotCondition = false;
        private Dictionary<String, Func<DiscountCondition>> ConditionFunctions;
        Dictionary<int, String> IntToDay;

        public DiscountParser(String discountString, String conditionString)        {
            DiscountString = discountString.Trim();
            ConditionString = conditionString.Trim();
            InitConditionFunctions();
            
            
        }

        private void InitIntToDay()
        {
            IntToDay = new Dictionary<int, String>();
            IntToDay[1] = "Sunday";
            IntToDay[2] = "Monday";
            IntToDay[3] = "Tuesday";
            IntToDay[4] = "Wednesday";
            IntToDay[5] = "Thursday";
            IntToDay[6] = "Friday";
            IntToDay[7] = "Saturday";
        }

        private void InitConditionFunctions()
        {
            ConditionFunctions = new Dictionary<string, Func<DiscountCondition>>();
            ConditionFunctions["DayOfWeek"] = ParseDayOfWeek;
            ConditionFunctions["Hour"] = ParseHour;
            ConditionFunctions["TotalBasketPriceRange"] = ParseBasketRange;
            ConditionFunctions["TotalBasketPriceFrom"] = ParseBasketFrom;
            ConditionFunctions["TotalBasketPriceTo"] = ParseBasketTo;
            ConditionFunctions["ItemTotalAmountInBasketRange"] = ParseItemAmountRange;
            ConditionFunctions["ItemTotalAmountInBasketFrom"] = ParseItemAmountFrom;
            ConditionFunctions["ItemTotalAmountInBasketTo"] = ParseItemAmountTo;
            ConditionFunctions["CategoryTotalAmountInBasketRange"] = ParseCategoryAmountRange;
            ConditionFunctions["CategoryTotalAmountInBasketFrom"] = ParseCategoryAmountFrom;
            ConditionFunctions["CategoryTotalAmountInBasketTo"] = ParseCategoryAmountTo;
        }

        public Discount Parse()
        {
            DiscountCondition conditionParsed = ParseCondition();
            return ParseDiscount(conditionParsed);
        }

        private DiscountCondition ParseCondition()
        {
            if (ConditionString[0] != '(')
            {
                return ParseSingleCondition();
            }
            return null;
        }

        private DiscountCondition ParseSingleCondition()
        {
            String conditionName = "";
            while (ConditionIndex < ConditionString.Length)
            {
                if (ConditionString[ConditionIndex] == '_')
                {
                    if (ConditionFunctions.ContainsKey(conditionName))
                    {
                        ConditionIndex++; // Calling parsing functions with the next index.
                        return ConditionFunctions[conditionName]();
                    }
                    else
                    {
                        throw new Exception($"Parsing condition string failed. '{conditionName}' is not a valid condition type.");
                    }
                }
                else
                {
                    conditionName += ConditionString[ConditionIndex];
                }
                ConditionIndex++;
            }
            throw new Exception("Parsing condition string failed. Condition string format is wrong.");
        }

        private DiscountCondition ParseDayOfWeek()
        {
            int day = -1;
            String dayString = "";
            while (ConditionIndex < ConditionString.Length && (ConditionString[ConditionIndex] != ')' || ConditionString[ConditionIndex] != ' '))
            {
                dayString += ConditionString[ConditionIndex];
                ConditionIndex++;
            }
            ConditionIndex++;

            try
            {
                day = int.Parse(dayString.Trim());
            }
            catch (Exception)
            {
                throw new Exception($"Parsing condition string failed. Unable to parse '{dayString}' as an integer.");
            }

            if (day < 1 || day > 7)
            {
                throw new Exception($"Parsing condition string failed. '{day}' has to be in the range [1-7].");
            }

            return new DayOnWeekCondition(IntToDay[day], NotCondition);
        }

        private DiscountCondition ParseHour()
        {
            return null;
        }

        private DiscountCondition ParseBasketRange()
        {
            return null;
        }

        private DiscountCondition ParseBasketFrom()
        {
            return null;
        }

        private DiscountCondition ParseBasketTo()
        {
            return null;
        }

        private DiscountCondition ParseItemAmountRange()
        {
            return null;
        }

        private DiscountCondition ParseItemAmountFrom()
        {
            return null;
        }

        private DiscountCondition ParseItemAmountTo()
        {
            return null;
        }

        private DiscountCondition ParseCategoryAmountRange()
        {
            return null;
        }

        private DiscountCondition ParseCategoryAmountFrom()
        {
            return null;
        }

        private DiscountCondition ParseCategoryAmountTo()
        {
            return null;
        }

        private Discount ParseDiscount(DiscountCondition condition)
        {
            return null;
        }
    }
}
