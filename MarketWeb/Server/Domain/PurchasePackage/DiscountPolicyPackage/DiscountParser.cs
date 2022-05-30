using MarketWeb.Server.Domain.PurchasePackage.DiscountPackage;
using System;
using System.Collections.Generic;

namespace MarketWeb.Server.Domain.PurchasePackage.DiscountPolicyPackage
{
    public class DiscountParser
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Discount Variables
        private String DiscountString;
        private int DiscountIndex = 0;
        private Dictionary<String, Func<Discount>> DiscountFunctions;
        private Dictionary<String, Func<Discount>> DiscountOperationsFunctions;
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

        // Condition Variables
        private DiscountCondition ParsedCondition = null;
        private String ConditionString;
        private int ConditionIndex = 0;
        private bool NotCondition = false;
        private Dictionary<String, Func<DiscountCondition>> ConditionFunctions;
        private Dictionary<int, String> IntToDay;
        private Dictionary<String, Func<DiscountCondition>> LogicalOperationsFunctions;
        private class NameAndValue
        {
            public String Name { get; private set; }
            public int Value { get; private set; }

            public NameAndValue(String name, int value)
            {
                Name = name; Value = value;
            }
        }
        private class NameAndRange
        {
            public String Name { get; private set; }
            public int From { get; private set; }
            public int To { get; private set; }

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
            InitLogicalOperationsFunctions();
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
            DiscountFunctions["ItemPercentage"] = ParseItemPerecentageDiscount;
            DiscountFunctions["CategoryPercentage"] = ParseCategoryPerecentageDiscount;
            DiscountFunctions["BasketPerecentage"] = ParseBasketPercentageDiscount;
            DiscountFunctions["BasketAbsolute"] = ParseBasketNumericDiscount;
        }

        private void InitLogicalOperationsFunctions()
        {
            LogicalOperationsFunctions = new Dictionary<string, Func<DiscountCondition>>();
            LogicalOperationsFunctions["NOT"] = ParseLogicalNot;
            LogicalOperationsFunctions["AND"] = ParseLogicalAnd;
            LogicalOperationsFunctions["OR"] = ParseLogicalOr;
            LogicalOperationsFunctions["XOR"] = ParseLogicalXor;
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
            if (ConditionString.Length > 0)
            {
                if (ConditionString[0] != '(')
                {
                    ParsedCondition = ParseSingleCondition();
                }
                else
                {
                    ConditionIndex++;
                    ParsedCondition = ParseCondition();
                }
            }
            if (ParsedCondition != null)
                _logger.Error(ParsedCondition.GetConditionString(0));
            return ParseDiscount();
        }

        private DiscountCondition ParseCondition()
        {
            if (ConditionIndex >= ConditionString.Length)
            {
                throw new Exception($"Parsing condition string failed. Condition has no closing bracket.");
            }

            return ParseLogicalOperation();
        }

        private DiscountCondition ParseLogicalOperation()
        {
            String op = "";
            if (ConditionIndex < ConditionString.Length)
            {
                while (ConditionString[ConditionIndex] == ' ') //Ignore whitespace
                {
                    ConditionIndex++;
                }
                while (ConditionString[ConditionIndex] != ' ')
                {
                    op += ConditionString[ConditionIndex];
                    ConditionIndex++;
                }
                if (LogicalOperationsFunctions.ContainsKey(op))
                {
                    ConditionIndex++; // Calling parsing functions with the next index.
                    return LogicalOperationsFunctions[op]();
                }
                else
                {
                    throw new Exception($"Parsing condition string failed. '{op}' is not a valid logical operation.");
                }
            }
            throw new Exception($"Parsing condition string failed. '{op}' cannot be parsed as a logical operation.");
        }

        private DiscountCondition ParseLogicalNot()
        {
            while (ConditionString[ConditionIndex] == ' ')
            {
                ConditionIndex++;
            }

            NotCondition = true;
            DiscountCondition condition = ParseSingleCondition();
            NotCondition = false;

            while (ConditionString[ConditionIndex] == ' ')
            {
                ConditionIndex++;
            }

            if (ConditionString[ConditionIndex] != ')')
            {
                throw new Exception($"Parsing condition string failed. Expected only one expression in a NOT format: index={ConditionIndex}.");
            }

            ConditionIndex++;

            while (ConditionString[ConditionIndex] == ' ')
            {
                ConditionIndex++;
            }

            return condition;
        }

        private List<DiscountCondition> ParseListOfConditions()
        {
            List<DiscountCondition> conditions = new List<DiscountCondition>();
            while (ConditionIndex < ConditionString.Length && ConditionString[ConditionIndex] != ')')
            {
                if (ConditionString[ConditionIndex] == '(')
                {
                    ConditionIndex++;
                    conditions.Add(ParseCondition());
                }
                else
                {
                    conditions.Add(ParseSingleCondition());
                }
            }
            if (ConditionIndex < ConditionString.Length - 1)
                ConditionIndex++;

            return conditions;
        }

        private AndComposition ParseLogicalAnd()
        {
            List<DiscountCondition> conditions = ParseListOfConditions();

            if (conditions.Count == 0)
            {
                throw new Exception($"Parsing condition string failed. One of the ANDs has no conditions.");
            }

            return new AndComposition(NotCondition, conditions);
        }

        private OrComposition ParseLogicalOr()
        {
            List<DiscountCondition> conditions = ParseListOfConditions();

            if (conditions.Count == 0)
            {
                throw new Exception($"Parsing condition string failed. One of the ORs has no conditions.");
            }

            return new OrComposition(NotCondition, conditions);
        }

        private XorComposition ParseLogicalXor()
        {
            List<DiscountCondition> conditions = ParseListOfConditions();

            if (conditions.Count == 0)
            {
                throw new Exception($"Parsing condition string failed. One of the XORs has no conditions.");
            }

            return new XorComposition(NotCondition, conditions);
        }

        private DiscountCondition ParseSingleCondition()
        {
            String conditionName = "";
            if (ConditionIndex < ConditionString.Length)
            {
                while (ConditionString[ConditionIndex] == ' ') //Ignore whitespace
                {
                    ConditionIndex++;
                }
                while (ConditionString[ConditionIndex] != '_')
                {
                    conditionName += ConditionString[ConditionIndex];
                    ConditionIndex++;
                }
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
            throw new Exception("Parsing condition string failed. Condition string format is wrong.");
        }

        private int ParseOneIntegerCondition()
        {
            int value = -1;
            String valueString = "";
            while (ConditionIndex < ConditionString.Length && (ConditionString[ConditionIndex] != ')' && ConditionString[ConditionIndex] != ' ' && ConditionString[ConditionIndex] != '_'))
            {
                valueString += ConditionString[ConditionIndex];
                ConditionIndex++;
            }
            if (ConditionIndex < ConditionString.Length - 1 && ConditionString[ConditionIndex] != ')')
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
            while (ConditionIndex < ConditionString.Length && (ConditionString[ConditionIndex] != ')' && ConditionString[ConditionIndex] != ' ' && ConditionString[ConditionIndex] != '_'))
            {
                fromString += ConditionString[ConditionIndex];
                ConditionIndex++;
            }
            if (ConditionString[ConditionIndex] == ')' || (ConditionIndex > 0 && ConditionString[ConditionIndex - 1] == ')'))
            {
                throw new Exception($"Parsing condition string failed. At index {ConditionIndex}.");
            }
            ConditionIndex++;
            while (ConditionIndex < ConditionString.Length && (ConditionString[ConditionIndex] != ')' && ConditionString[ConditionIndex] != ' '))
            {
                toString += ConditionString[ConditionIndex];
                ConditionIndex++;
            }
            if (ConditionIndex < ConditionString.Length - 1 && ConditionString[ConditionIndex] != ')')
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
            while (ConditionIndex < ConditionString.Length && (ConditionString[ConditionIndex] != ')' && ConditionString[ConditionIndex] != '_'))
            {
                nameString += ConditionString[ConditionIndex];
                ConditionIndex++;
            }
            if (ConditionString[ConditionIndex] == ')' || (ConditionIndex > 0 && ConditionString[ConditionIndex - 1] == ')'))
            {
                throw new Exception($"Parsing condition string failed. At index {ConditionIndex}.");
            }
            ConditionIndex++;
            while (ConditionIndex < ConditionString.Length && (ConditionString[ConditionIndex] != ')' && ConditionString[ConditionIndex] != ' '))
            {
                valueString += ConditionString[ConditionIndex];
                ConditionIndex++;
            }
            if (ConditionIndex < ConditionString.Length - 1 && ConditionString[ConditionIndex] != ')')
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
            while (ConditionIndex < ConditionString.Length && (ConditionString[ConditionIndex] != ')' && ConditionString[ConditionIndex] != '_'))
            {
                nameString += ConditionString[ConditionIndex];
                ConditionIndex++;
            }
            if (ConditionString[ConditionIndex] == ')' || (ConditionIndex > 0 && ConditionString[ConditionIndex - 1] == ')'))
            {
                throw new Exception($"Parsing condition string failed. At index {ConditionIndex}.");
            }
            ConditionIndex++;
            while (ConditionIndex < ConditionString.Length && (ConditionString[ConditionIndex] != ')' && ConditionString[ConditionIndex] != '_'))
            {
                fromString += ConditionString[ConditionIndex];
                ConditionIndex++;
            }
            if (ConditionString[ConditionIndex] == ')' || (ConditionIndex > 0 && ConditionString[ConditionIndex - 1] == ')'))
            {
                throw new Exception($"Parsing condition string failed. At index {ConditionIndex}.");
            }
            ConditionIndex++;
            while (ConditionIndex < ConditionString.Length && (ConditionString[ConditionIndex] != ')' && ConditionString[ConditionIndex] != ' '))
            {
                toString += ConditionString[ConditionIndex];
                ConditionIndex++;
            }
            if (ConditionIndex < ConditionString.Length - 1 && ConditionString[ConditionIndex] != ')')
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

            return new DayOnWeekCondition(IntToDay[day], NotCondition);
        }

        private HourCondition ParseHour()
        {
            int[] from_to = ParseTwoIntegersCondition();

            return new HourCondition(from_to[0], from_to[1], NotCondition);
        }

        private PriceableCondition ParseBasketRange()
        {
            int[] from_to = ParseTwoIntegersCondition();

            return new PriceableCondition(null, from_to[0], from_to[1], NotCondition);
        }

        private PriceableCondition ParseBasketFrom()
        {
            int from = ParseOneIntegerCondition();

            return new PriceableCondition(null, from, -1, NotCondition);
        }

        private PriceableCondition ParseBasketTo()
        {
            int to = ParseOneIntegerCondition();

            return new PriceableCondition(null, -1, to, NotCondition);
        }

        private SearchItemCondition ParseItemAmountRange()
        {
            NameAndRange nar = ParseNameAndRangeCondition();

            return new SearchItemCondition(nar.Name, nar.From, nar.To, NotCondition);
        }

        private SearchItemCondition ParseItemAmountFrom()
        {
            NameAndValue nav = ParseNameAndValueCondition();

            return new SearchItemCondition(nav.Name, nav.Value, -1, NotCondition);
        }

        private SearchItemCondition ParseItemAmountTo()
        {
            NameAndValue nav = ParseNameAndValueCondition();

            return new SearchItemCondition(nav.Name, -1, nav.Value, NotCondition);
        }

        private SearchCategoryCondition ParseCategoryAmountRange()
        {
            NameAndRange nar = ParseNameAndRangeCondition();

            return new SearchCategoryCondition(nar.Name, nar.From, nar.To, NotCondition);
        }

        private SearchCategoryCondition ParseCategoryAmountFrom()
        {
            NameAndValue nav = ParseNameAndValueCondition();

            return new SearchCategoryCondition(nav.Name, nav.Value, -1, NotCondition);
        }

        private SearchCategoryCondition ParseCategoryAmountTo()
        {
            NameAndValue nav = ParseNameAndValueCondition();

            return new SearchCategoryCondition(nav.Name, -1, nav.Value, NotCondition);
        }

        /*
         * /\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\
         *              CONDITION LOGIC
         *              DISCOUNT LOGIC
         * \/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/
         */

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
            List<Discount> discounts = ParseListOfDiscounts();

            if (discounts.Count == 0)
            {
                throw new Exception($"Parsing discount string failed. One of the PLUSs has no discounts.");
            }

            if (ParsedCondition != null)
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
            List<Discount> discounts = ParseListOfDiscounts();

            if (discounts.Count == 0)
            {
                throw new Exception($"Parsing discount string failed. One of the MAXs has no discounts.");
            }

            if (ParsedCondition != null)
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
            if (DiscountString[DiscountIndex] == ')' || (DiscountIndex > 0 && DiscountString[DiscountIndex - 1] == ')'))
            {
                throw new Exception($"Parsing discount string failed. At index {DiscountIndex}.");
            }
            DiscountIndex++;
            while (DiscountIndex < DiscountString.Length && (DiscountString[DiscountIndex] != ')' && DiscountString[DiscountIndex] != '_'))
            {
                valueString += DiscountString[DiscountIndex];
                DiscountIndex++;
            }
            if (DiscountString[DiscountIndex] == ')' || (DiscountIndex > 0 && DiscountString[DiscountIndex - 1] == ')'))
            {
                throw new Exception($"Parsing discount string failed. At index {DiscountIndex}.");
            }
            DiscountIndex++;
            while (DiscountIndex < DiscountString.Length && (DiscountString[DiscountIndex] != ')' && DiscountString[DiscountIndex] != ' '))
            {
                yearString += DiscountString[DiscountIndex];
                DiscountIndex++;
            }
            if (DiscountString[DiscountIndex] == ')' || (DiscountIndex > 0 && DiscountString[DiscountIndex - 1] == ')'))
            {
                throw new Exception($"Parsing discount string failed. At index {DiscountIndex}.");
            }
            DiscountIndex++;
            while (DiscountIndex < DiscountString.Length && (DiscountString[DiscountIndex] != ')' && DiscountString[DiscountIndex] != ' '))
            {
                monthString += DiscountString[DiscountIndex];
                DiscountIndex++;
            }
            if (DiscountString[DiscountIndex] == ')' || (DiscountIndex > 0 && DiscountString[DiscountIndex - 1] == ')'))
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
            if (DiscountString[DiscountIndex] == ')' || (DiscountIndex > 0 && DiscountString[DiscountIndex - 1] == ')'))
            {
                throw new Exception($"Parsing discount string failed. At index {DiscountIndex}.");
            }
            DiscountIndex++;
            while (DiscountIndex < DiscountString.Length && (DiscountString[DiscountIndex] != ')' && DiscountString[DiscountIndex] != ' '))
            {
                yearString += DiscountString[DiscountIndex];
                DiscountIndex++;
            }
            if (DiscountString[DiscountIndex] == ')' || (DiscountIndex > 0 && DiscountString[DiscountIndex - 1] == ')'))
            {
                throw new Exception($"Parsing discount string failed. At index {DiscountIndex}.");
            }
            DiscountIndex++;
            while (DiscountIndex < DiscountString.Length && (DiscountString[DiscountIndex] != ')' && DiscountString[DiscountIndex] != ' '))
            {
                monthString += DiscountString[DiscountIndex];
                DiscountIndex++;
            }
            if (DiscountString[DiscountIndex] == ')' || (DiscountIndex > 0 && DiscountString[DiscountIndex - 1] == ')'))
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

        private ItemDiscount ParseItemPerecentageDiscount()
        {
            NameValueDate nvd = ParseNameValueDateDiscount();

            if (ParsedCondition != null)
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

            if (ParsedCondition != null)
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

            if (ParsedCondition != null)
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

            if (ParsedCondition != null)
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
