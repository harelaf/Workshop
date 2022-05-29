using MarketWeb.Server.Domain.PurchasePackage.DiscountPackage;
using System;
using System.Collections.Generic;

namespace MarketWeb.Server.Domain.PurchasePackage.DiscountPolicyPackage
{
    public class DiscountParser
    {
        // Discount Variables
        private String DiscountString;

        // Condition Variables
        private String ConditionString;
        private int ConditionIndex = 0;
        private bool NotCondition = false;
        private Dictionary<String, Func<DiscountCondition>> ConditionFunctions;
        Dictionary<int, String> IntToDay;
        private class NameAndValue
        {
            public String Name { get; }
            public int Value { get; }

            public NameAndValue(String name, int value)
            {
                Name = name; Value = value;
            }
        }
        private class NameAndRange
        {
            public String Name { get; }
            public int From { get; }
            public int To { get; }

            public NameAndRange(String name, int from, int to)
            {
                Name = name; From = from; To = to; 
            }
        }

        public DiscountParser(String discountString, String conditionString)        {
            DiscountString = discountString.Trim();
            ConditionString = conditionString.Trim();
            InitConditionFunctions();
            InitIntToDay();
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

        private int ParseOneIntegerCondition()
        {
            int value = -1;
            String valueString = "";
            while (ConditionIndex < ConditionString.Length && (ConditionString[ConditionIndex] != ')' || ConditionString[ConditionIndex] != ' ' || ConditionString[ConditionIndex] != '_'))
            {
                valueString += ConditionString[ConditionIndex];
                ConditionIndex++;
            }
            ConditionIndex++;

            try
            {
                value = int.Parse(valueString.Trim());
            }
            catch (Exception)
            {
                throw new Exception($"Parsing condition string failed. Unable to parse '{valueString}' as integer.");
            }

            return value;
        }

        private int[] ParseTwoIntegersCondition()
        {
            int from = -1;
            int to = -1;
            String fromString = "";
            String toString = "";
            while (ConditionIndex < ConditionString.Length && (ConditionString[ConditionIndex] != ')' || ConditionString[ConditionIndex] != ' ' || ConditionString[ConditionIndex] != '_'))
            {
                fromString += ConditionString[ConditionIndex];
                ConditionIndex++;
            }
            if (ConditionString[ConditionIndex] == ')' || (ConditionIndex > 0 && ConditionString[ConditionIndex - 1] == ')'))
            {
                throw new Exception($"Parsing condition string failed. At index {ConditionIndex}.");
            }
            while (ConditionIndex < ConditionString.Length && (ConditionString[ConditionIndex] != ')' || ConditionString[ConditionIndex] != ' '))
            {
                toString += ConditionString[ConditionIndex];
                ConditionIndex++;
            }
            ConditionIndex++;

            try
            {
                from = int.Parse(fromString.Trim());
                to = int.Parse(toString.Trim());
            }
            catch (Exception)
            {
                throw new Exception($"Parsing condition string failed. Unable to parse '{fromString}' or '{toString}' as integers.");
            }

            return new int[] { from, to };
        }

        private NameAndValue ParseNameAndValueCondition()
        {
            int value = -1;
            String nameString = "";
            String valueString = "";
            while (ConditionIndex < ConditionString.Length && (ConditionString[ConditionIndex] != ')' || ConditionString[ConditionIndex] != '_'))
            {
                nameString += ConditionString[ConditionIndex];
                ConditionIndex++;
            }
            if (ConditionString[ConditionIndex] == ')' || (ConditionIndex > 0 && ConditionString[ConditionIndex - 1] == ')'))
            {
                throw new Exception($"Parsing condition string failed. At index {ConditionIndex}.");
            }
            while (ConditionIndex < ConditionString.Length && (ConditionString[ConditionIndex] != ')' || ConditionString[ConditionIndex] != ' '))
            {
                valueString += ConditionString[ConditionIndex];
                ConditionIndex++;
            }
            ConditionIndex++;

            try
            {
                value = int.Parse(valueString.Trim());
            }
            catch (Exception)
            {
                throw new Exception($"Parsing condition string failed. Unable to parse '{valueString}' as integer.");
            }

            return new NameAndValue(nameString, value);
        }

        private NameAndRange ParseNameAndRangeCondition()
        {
            int from = -1;
            int to = -1;
            String nameString = "";
            String fromString = "";
            String toString = "";
            while (ConditionIndex < ConditionString.Length && (ConditionString[ConditionIndex] != ')' || ConditionString[ConditionIndex] != '_'))
            {
                nameString += ConditionString[ConditionIndex];
                ConditionIndex++;
            }
            if (ConditionString[ConditionIndex] == ')' || (ConditionIndex > 0 && ConditionString[ConditionIndex - 1] == ')'))
            {
                throw new Exception($"Parsing condition string failed. At index {ConditionIndex}.");
            }
            while (ConditionIndex < ConditionString.Length && (ConditionString[ConditionIndex] != ')' || ConditionString[ConditionIndex] != ' '))
            {
                fromString += ConditionString[ConditionIndex];
                ConditionIndex++;
            }
            if (ConditionString[ConditionIndex] == ')' || (ConditionIndex > 0 && ConditionString[ConditionIndex - 1] == ')'))
            {
                throw new Exception($"Parsing condition string failed. At index {ConditionIndex}.");
            }
            while (ConditionIndex < ConditionString.Length && (ConditionString[ConditionIndex] != ')' || ConditionString[ConditionIndex] != ' '))
            {
                toString += ConditionString[ConditionIndex];
                ConditionIndex++;
            }
            ConditionIndex++;

            try
            {
                from = int.Parse(fromString.Trim());
                to = int.Parse(toString.Trim());
            }
            catch (Exception)
            {
                throw new Exception($"Parsing condition string failed. Unable to parse '{fromString}' or '{toString}' as integers.");
            }

            return new NameAndRange(nameString, from, to);
        }

        private DayOnWeekCondition ParseDayOfWeek()
        {
            int day = ParseOneIntegerCondition();

            if (day < 1 || day > 7)
            {
                throw new Exception($"Parsing condition string failed. '{day}' has to be in the range [1-7].");
            }

            bool currentNot = NotCondition;
            if (ConditionString[ConditionIndex - 1] == ')')
            {
                NotCondition = false;
            }
            
            return new DayOnWeekCondition(IntToDay[day], currentNot);
        }

        private HourCondition ParseHour()
        {
            int[] from_to = ParseTwoIntegersCondition();

            bool currentNot = NotCondition;
            if (ConditionString[ConditionIndex - 1] == ')')
            {
                NotCondition = false;
            }

            return new HourCondition(from_to[0], from_to[1], currentNot);
        }

        private PriceableCondition ParseBasketRange()
        {
            int[] from_to = ParseTwoIntegersCondition();

            bool currentNot = NotCondition;
            if (ConditionString[ConditionIndex - 1] == ')')
            {
                NotCondition = false;
            }

            return new PriceableCondition(null, from_to[0], from_to[1], currentNot);
        }

        private DiscountCondition ParseBasketFrom()
        {
            int from = ParseOneIntegerCondition();

            bool currentNot = NotCondition;
            if (ConditionString[ConditionIndex - 1] == ')')
            {
                NotCondition = false;
            }

            return new PriceableCondition(null, from, -1, currentNot);
        }

        private DiscountCondition ParseBasketTo()
        {
            int to = ParseOneIntegerCondition();

            bool currentNot = NotCondition;
            if (ConditionString[ConditionIndex - 1] == ')')
            {
                NotCondition = false;
            }

            return new PriceableCondition(null, -1, to, currentNot);
        }

        private SearchItemCondition ParseItemAmountRange()
        {
            NameAndRange nar = ParseNameAndRangeCondition();

            bool currentNot = NotCondition;
            if (ConditionString[ConditionIndex - 1] == ')')
            {
                NotCondition = false;
            }

            return new SearchItemCondition(nar.Name, nar.From, nar.To, currentNot);
        }

        private SearchItemCondition ParseItemAmountFrom()
        {
            NameAndValue nar = ParseNameAndValueCondition();

            bool currentNot = NotCondition;
            if (ConditionString[ConditionIndex - 1] == ')')
            {
                NotCondition = false;
            }

            return new SearchItemCondition(nar.Name, nar.Value, -1, currentNot);
        }

        private SearchItemCondition ParseItemAmountTo()
        {
            NameAndValue nar = ParseNameAndValueCondition();

            bool currentNot = NotCondition;
            if (ConditionString[ConditionIndex - 1] == ')')
            {
                NotCondition = false;
            }

            return new SearchItemCondition(nar.Name, -1, nar.Value, currentNot);
        }

        private SearchCategoryCondition ParseCategoryAmountRange()
        {
            NameAndRange nar = ParseNameAndRangeCondition();

            bool currentNot = NotCondition;
            if (ConditionString[ConditionIndex - 1] == ')')
            {
                NotCondition = false;
            }

            return new SearchCategoryCondition(nar.Name, nar.From, nar.To, currentNot);
        }

        private SearchCategoryCondition ParseCategoryAmountFrom()
        {
            NameAndValue nar = ParseNameAndValueCondition();

            bool currentNot = NotCondition;
            if (ConditionString[ConditionIndex - 1] == ')')
            {
                NotCondition = false;
            }

            return new SearchCategoryCondition(nar.Name, nar.Value, -1, currentNot);
        }

        private DiscountCondition ParseCategoryAmountTo()
        {
            NameAndValue nar = ParseNameAndValueCondition();

            bool currentNot = NotCondition;
            if (ConditionString[ConditionIndex - 1] == ')')
            {
                NotCondition = false;
            }

            return new SearchCategoryCondition(nar.Name, -1, nar.Value, currentNot);
        }

        /*
         * /\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\
         *              CONDITION LOGIC
         *              DISCOUNT LOGIC
         * \/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/
         */

        private Discount ParseDiscount(DiscountCondition condition)
        {
            return null;
        }
    }
}
