using MarketWeb.Server.Domain.PolicyPackage;
using System;
using System.Collections.Generic;

namespace MarketWeb.Server.Domain.PurchasePackage.PolicyPackage
{
    public class ConditionParser
    {
        // Condition Variables
        private Condition ParsedCondition = null;
        private String ConditionString;
        private int ConditionIndex = 0;
        private bool NotCondition = false;
        private Dictionary<String, Func<Condition>> ConditionFunctions;
        private Dictionary<int, String> IntToDay;
        private Dictionary<String, Func<Condition>> LogicalOperationsFunctions;
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

        public ConditionParser(String conditionString)
        {
            if (conditionString == null)
                conditionString = "";
            ConditionString = conditionString.Trim();
            InitConditionFunctions();
            InitIntToDay();
            InitLogicalOperationsFunctions();
        }

        private void InitLogicalOperationsFunctions()
        {
            LogicalOperationsFunctions = new Dictionary<string, Func<Condition>>();
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
            ConditionFunctions = new Dictionary<string, Func<Condition>>();
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

        public Condition Parse()
        {
            if (ConditionString.Length > 0)
            {
                if (ConditionString[0] != '(')
                {
                    return ParseSingleCondition();
                }
                else
                {
                    ConditionIndex++;
                    return ParseCondition();
                }
            }
            return null;
        }

        private Condition ParseCondition()
        {
            if (ConditionIndex >= ConditionString.Length)
            {
                throw new Exception($"Parsing condition string failed. Condition has no closing bracket.");
            }

            return ParseLogicalOperation();
        }

        private Condition ParseLogicalOperation()
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

        private Condition ParseLogicalNot()
        {
            while (ConditionString[ConditionIndex] == ' ')
            {
                ConditionIndex++;
            }

            NotCondition = true;
            Condition condition = ParseSingleCondition();
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

        private List<Condition> ParseListOfConditions()
        {
            List<Condition> conditions = new List<Condition>();
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
            List<Condition> conditions = ParseListOfConditions();

            if (conditions.Count == 0)
            {
                throw new Exception($"Parsing condition string failed. One of the ANDs has no conditions.");
            }

            return new AndComposition(NotCondition, conditions);
        }

        private OrComposition ParseLogicalOr()
        {
            List<Condition> conditions = ParseListOfConditions();

            if (conditions.Count == 0)
            {
                throw new Exception($"Parsing condition string failed. One of the ORs has no conditions.");
            }

            return new OrComposition(NotCondition, conditions);
        }

        private XorComposition ParseLogicalXor()
        {
            List<Condition> conditions = ParseListOfConditions();

            if (conditions.Count == 0)
            {
                throw new Exception($"Parsing condition string failed. One of the XORs has no conditions.");
            }

            return new XorComposition(NotCondition, conditions);
        }

        private Condition ParseSingleCondition()
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
    }
}
