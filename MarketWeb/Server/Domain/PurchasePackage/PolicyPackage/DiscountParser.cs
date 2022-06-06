using MarketWeb.Server.Domain.PolicyPackage;
using MarketWeb.Server.Domain.PurchasePackage.PolicyPackage;
using System;
using System.Collections.Generic;

namespace MarketWeb.Server.Domain.PurchasePackage.DiscountPolicyPackage
{
    public class DiscountParser
    {
        // Discount Variables
        private String DiscountString;
        private int DiscountIndex = 0;
        private Dictionary<String, Func<Discount>> DiscountFunctions;
        private Dictionary<String, Func<Discount>> DiscountOperationsFunctions;
        private bool IsSingleCondition = false;
        private Condition ParsedCondition;
        private class NameValueDate
        {
            public String Name { get; private set; }
            public int Value { get; private set; }
            public int Year { get; private set; }
            public int Month { get; private set; }
            public int Day { get; private set; }

            public NameValueDate(string name, int value, int year, int month, int day)
            {
                Name = name;
                Value = value;
                Year = year;
                Month = month;
                Day = day;
            }
        }
        private class ValueDate
        {
            public int Value { get; private set; }
            public int Year { get; private set; }
            public int Month { get; private set; }
            public int Day { get; private set; }

            public ValueDate(int value, int year, int month, int day)
            {
                Value = value;
                Year = year;
                Month = month;
                Day = day;
            }
        }

        public DiscountParser(String discountString, String conditionString)
        {
            ParsedCondition = new ConditionParser(conditionString).Parse();
            DiscountString = discountString.Trim();
            InitDiscountFunctions();
            InitDiscountOperationsFunctions();
        }

        private void InitDiscountOperationsFunctions()
        {
            DiscountOperationsFunctions = new Dictionary<string, Func<Discount>>();
            DiscountOperationsFunctions["PLUS"] = ParsePlusOperation;
            DiscountOperationsFunctions["MAX"] = ParseMaxOperation;
        }

        private void InitDiscountFunctions()
        {
            DiscountFunctions = new Dictionary<string, Func<Discount>>();
            DiscountFunctions["ItemPercentage"] = ParseItemPercentageDiscount;
            DiscountFunctions["CategoryPercentage"] = ParseCategoryPerecentageDiscount;
            DiscountFunctions["BasketPercentage"] = ParseBasketPercentageDiscount;
            DiscountFunctions["BasketAbsolute"] = ParseBasketNumericDiscount;
        }

        public Discount Parse()
        {
            Discount discount = null;
            if (DiscountString[0] != '(')
            {
                IsSingleCondition = true;
                discount = ParseSingleDiscount();
            }
            else
            {
                DiscountIndex++;
                discount = ParseDiscount();
            }
            //_logger.Error(discount.GetDiscountString(0));
            return discount;
        }

        private Discount ParseDiscount()
        {
            if (DiscountIndex >= DiscountString.Length)
            {
                throw new Exception($"Parsing discount string failed. Discount has no closing bracket.");
            }

            return ParseDiscountOperation();
        }

        private Discount ParseDiscountOperation()
        {
            String op = "";
            if (DiscountIndex < DiscountString.Length)
            {
                while (DiscountString[DiscountIndex] == ' ') //Ignore whitespace
                {
                    DiscountIndex++;
                }
                while (DiscountString[DiscountIndex] != ' ')
                {
                    op += DiscountString[DiscountIndex];
                    DiscountIndex++;
                }
                if (DiscountOperationsFunctions.ContainsKey(op))
                {
                    DiscountIndex++; // Calling parsing functions with the next index.
                    return DiscountOperationsFunctions[op]();
                }
                else
                {
                    throw new Exception($"Parsing discount string failed. '{op}' is not a valid operation.");
                }
            }
            throw new Exception($"Parsing discount string failed. '{op}' cannot be parsed as an operation.");
        }

        private List<Discount> ParseListOfDiscounts()
        {
            List<Discount> discounts = new List<Discount>();
            while (DiscountIndex < DiscountString.Length && DiscountString[DiscountIndex] != ')')
            {
                if (DiscountString[DiscountIndex] == '(')
                {
                    DiscountIndex++;
                    discounts.Add(ParseDiscount());
                }
                else
                {
                    discounts.Add(ParseSingleDiscount());
                }
            }
            if (DiscountIndex < DiscountString.Length - 1)
                DiscountIndex++;

            return discounts;
        }

        private PlusDiscount ParsePlusOperation()
        {
            int advanced = 0;
            while (DiscountString[DiscountIndex] == ' ')
            {
                DiscountIndex++;
                advanced++;
            }
            bool IsOutsideMost = ("(".Length + "PLUS".Length + " ".Length) + advanced == DiscountIndex;
            List<Discount> discounts = ParseListOfDiscounts();

            if (discounts.Count == 0)
            {
                throw new Exception($"Parsing discount string failed. One of the PLUSs has no discounts.");
            }

            if (ParsedCondition != null && IsOutsideMost)
            {
                return new PlusDiscount(discounts, ParsedCondition);
            }
            else
            {
                return new PlusDiscount(discounts);
            }
        }

        private MaxDiscount ParseMaxOperation() 
        {
            int advanced = 0;
            while (DiscountString[DiscountIndex] == ' ')
            {
                DiscountIndex++;
                advanced++;
            }
            bool IsOutsideMost = ("(".Length + "MAX".Length + " ".Length) + advanced == DiscountIndex;
            List<Discount> discounts = ParseListOfDiscounts();

            if (discounts.Count == 0)
            {
                throw new Exception($"Parsing discount string failed. One of the MAXs has no discounts.");
            }

            if (ParsedCondition != null && IsOutsideMost)
            {
                return new MaxDiscount(discounts, ParsedCondition);
            }
            else
            {
                return new MaxDiscount(discounts);
            }
        }

        private Discount ParseSingleDiscount()
        {
            String discountName = "";
            if (DiscountIndex < DiscountString.Length)
            {
                while (DiscountString[DiscountIndex] == ' ') //Ignore whitespace
                {
                    DiscountIndex++;
                }
                while (DiscountString[DiscountIndex] != '_')
                {
                    discountName += DiscountString[DiscountIndex];
                    DiscountIndex++;
                }
                if (DiscountFunctions.ContainsKey(discountName))
                {
                    DiscountIndex++; // Calling parsing functions with the next index.
                    return DiscountFunctions[discountName]();
                }
                else
                {
                    throw new Exception($"Parsing discount string failed. '{discountName}' is not a valid discount type.");
                }
            }
            throw new Exception("Parsing discount string failed. Condition string format is wrong.");
        }

        private NameValueDate ParseNameValueDateDiscount()
        {
            String nameString = "";
            int value = -1;
            String valueString = "";
            int year = -1;
            String yearString = "";
            int month = -1;
            String monthString = "";
            int day = -1;
            String dayString = "";
            while (DiscountIndex < DiscountString.Length && (DiscountString[DiscountIndex] != ')' && DiscountString[DiscountIndex] != '_'))
            {
                nameString += DiscountString[DiscountIndex];
                DiscountIndex++;
            }
            if (DiscountIndex >= DiscountString.Length || DiscountString[DiscountIndex] == ')' || (DiscountIndex > 0 && DiscountString[DiscountIndex - 1] == ')'))
            {
                throw new Exception($"Parsing discount string failed. At index {DiscountIndex}.");
            }
            DiscountIndex++;
            while (DiscountIndex < DiscountString.Length && (DiscountString[DiscountIndex] != ')' && DiscountString[DiscountIndex] != '_'))
            {
                valueString += DiscountString[DiscountIndex];
                DiscountIndex++;
            }
            if (DiscountIndex >= DiscountString.Length || DiscountString[DiscountIndex] == ')' || (DiscountIndex > 0 && DiscountString[DiscountIndex - 1] == ')'))
            {
                throw new Exception($"Parsing discount string failed. At index {DiscountIndex}.");
            }
            DiscountIndex++;
            while (DiscountIndex < DiscountString.Length && (DiscountString[DiscountIndex] != ')' && DiscountString[DiscountIndex] != '_'))
            {
                yearString += DiscountString[DiscountIndex];
                DiscountIndex++;
            }
            if (DiscountIndex >= DiscountString.Length || DiscountString[DiscountIndex] == ')' || (DiscountIndex > 0 && DiscountString[DiscountIndex - 1] == ')'))
            {
                throw new Exception($"Parsing discount string failed. At index {DiscountIndex}.");
            }
            DiscountIndex++;
            while (DiscountIndex < DiscountString.Length && (DiscountString[DiscountIndex] != ')' && DiscountString[DiscountIndex] != '_'))
            {
                monthString += DiscountString[DiscountIndex];
                DiscountIndex++;
            }
            if (DiscountIndex >= DiscountString.Length || DiscountString[DiscountIndex] == ')' || (DiscountIndex > 0 && DiscountString[DiscountIndex - 1] == ')'))
            {
                throw new Exception($"Parsing discount string failed. At index {DiscountIndex}.");
            }
            DiscountIndex++;
            while (DiscountIndex < DiscountString.Length && (DiscountString[DiscountIndex] != ')' && DiscountString[DiscountIndex] != ' '))
            {
                dayString += DiscountString[DiscountIndex];
                DiscountIndex++;
            }
            if (DiscountIndex < DiscountString.Length - 1 && DiscountString[DiscountIndex] != ')')
                DiscountIndex++;

            try
            {
                value = int.Parse(valueString.Trim());
                year = int.Parse(yearString.Trim());
                month = int.Parse(monthString.Trim());
                day = int.Parse(dayString.Trim());
            }
            catch (Exception)
            {
                throw new Exception($"Parsing discount string failed. Unable to parse '{valueString}' or '{yearString}' or '{monthString}' or '{dayString}' as integers.");
            }

            return new NameValueDate(nameString, value, year, month, day);
        }

        private ValueDate ParseValueDateDiscount()
        {
            int value = -1;
            String valueString = "";
            int year = -1;
            String yearString = "";
            int month = -1;
            String monthString = "";
            int day = -1;
            String dayString = "";
            while (DiscountIndex < DiscountString.Length && (DiscountString[DiscountIndex] != ')' && DiscountString[DiscountIndex] != '_'))
            {
                valueString += DiscountString[DiscountIndex];
                DiscountIndex++;
            }
            if (DiscountIndex >= DiscountString.Length || DiscountString[DiscountIndex] == ')' || (DiscountIndex > 0 && DiscountString[DiscountIndex - 1] == ')'))
            {
                throw new Exception($"Parsing discount string failed. At index {DiscountIndex}.");
            }
            DiscountIndex++;
            while (DiscountIndex < DiscountString.Length && (DiscountString[DiscountIndex] != ')' && DiscountString[DiscountIndex] != '_'))
            {
                yearString += DiscountString[DiscountIndex];
                DiscountIndex++;
            }
            if (DiscountIndex >= DiscountString.Length || DiscountString[DiscountIndex] == ')' || (DiscountIndex > 0 && DiscountString[DiscountIndex - 1] == ')'))
            {
                throw new Exception($"Parsing discount string failed. At index {DiscountIndex}.");
            }
            DiscountIndex++;
            while (DiscountIndex < DiscountString.Length && (DiscountString[DiscountIndex] != ')' && DiscountString[DiscountIndex] != '_'))
            {
                monthString += DiscountString[DiscountIndex];
                DiscountIndex++;
            }
            if (DiscountIndex >= DiscountString.Length || DiscountString[DiscountIndex] == ')' || (DiscountIndex > 0 && DiscountString[DiscountIndex - 1] == ')'))
            {
                throw new Exception($"Parsing discount string failed. At index {DiscountIndex}.");
            }
            DiscountIndex++;
            while (DiscountIndex < DiscountString.Length && (DiscountString[DiscountIndex] != ')' && DiscountString[DiscountIndex] != ' '))
            {
                dayString += DiscountString[DiscountIndex];
                DiscountIndex++;
            }
            if (DiscountIndex < DiscountString.Length - 1 && DiscountString[DiscountIndex] != ')')
                DiscountIndex++;

            try
            {
                value = int.Parse(valueString.Trim());
                year = int.Parse(yearString.Trim());
                month = int.Parse(monthString.Trim());
                day = int.Parse(dayString.Trim());
            }
            catch (Exception)
            {
                throw new Exception($"Parsing discount string failed. Unable to parse '{valueString}' or '{yearString}' or '{monthString}' or '{dayString}' as integers.");
            }

            return new ValueDate(value, year, month, day);
        }

        private ItemDiscount ParseItemPercentageDiscount()
        {
            NameValueDate nvd = ParseNameValueDateDiscount();

            if (IsSingleCondition)
            {
                return new ItemDiscount(nvd.Value, nvd.Name, ParsedCondition, new DateTime(nvd.Year, nvd.Month, nvd.Day));
            }
            else
            {
                return new ItemDiscount(nvd.Value, nvd.Name, new DateTime(nvd.Year, nvd.Month, nvd.Day));
            }
        }

        private CategoryDiscount ParseCategoryPerecentageDiscount()
        {
            NameValueDate nvd = ParseNameValueDateDiscount();

            if (IsSingleCondition)
            {
                return new CategoryDiscount(nvd.Value, nvd.Name, ParsedCondition, new DateTime(nvd.Year, nvd.Month, nvd.Day));
            }
            else
            {
                return new CategoryDiscount(nvd.Value, nvd.Name, new DateTime(nvd.Year, nvd.Month, nvd.Day));
            }
        }

        private AllProductsDiscount ParseBasketPercentageDiscount()
        {
            ValueDate vd = ParseValueDateDiscount();

            if (IsSingleCondition)
            {
                return new AllProductsDiscount(vd.Value, ParsedCondition, new DateTime(vd.Year, vd.Month, vd.Day));
            }
            else
            {
                return new AllProductsDiscount(vd.Value, new DateTime(vd.Year, vd.Month, vd.Day));
            }
        }

        private NumericDiscount ParseBasketNumericDiscount()
        {
            ValueDate vd = ParseValueDateDiscount();

            if (IsSingleCondition)
            {
                return new NumericDiscount(vd.Value, ParsedCondition, new DateTime(vd.Year, vd.Month, vd.Day));
            }
            else
            {
                return new NumericDiscount(vd.Value, new DateTime(vd.Year, vd.Month, vd.Day));
            }
        }
    }
}
