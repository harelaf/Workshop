using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Server.DataLayer
{
    internal abstract class ConditionDAL
    {
        [Required]
        internal bool _negative;
        internal ConditionDAL(bool negative)
        {
            _negative = negative;
        }
    }
    internal abstract class ComposedConditionDAL : ConditionDAL
    {
        internal List<ConditionDAL> _conditionList;
        internal ComposedConditionDAL(List<ConditionDAL> conditionList, bool negative) : base(negative)
        {
            if (conditionList == null)
                _conditionList = new List<ConditionDAL>();
            else _conditionList = conditionList;
        }
    }
    internal abstract class AtomicConditionDAL : ConditionDAL
    {
        internal AtomicConditionDAL(bool negative) : base(negative)
        {
        }
    }
    internal abstract class SearchablePriceableDAL : AtomicConditionDAL
    {
        [Required]
        internal string _keyWord;
        [Required] 
        internal int _minValue;
        [Required] 
        internal int _maxValue;

        internal SearchablePriceableDAL(string keyWord, int minValue, int maxValue, bool negative) : base(negative)
        {
            _keyWord = keyWord;
            _minValue = minValue;
            _maxValue = maxValue;
        }

    }
    internal class SearchItemConditionDAL : SearchablePriceableDAL
    {
        internal SearchItemConditionDAL(string keyWord, int minValue, int maxValue, bool negative) : base(keyWord, minValue, maxValue, negative)
        {
        }
    }
    internal class SearchCategoryConditionDAL : SearchablePriceableDAL
    {
        internal SearchCategoryConditionDAL(string keyWord, int minValue, int maxValue, bool negative) : base(keyWord, minValue, maxValue, negative)
        {
        }
    }
    internal class PriceableConditionDAL : SearchablePriceableDAL
    {
        internal PriceableConditionDAL(int minValue, int maxValue, bool negative) : base("", minValue, maxValue, negative)
        {
        }
    }
    internal class HourConditionDAL : AtomicConditionDAL
    {
        [Required]
        internal int _minHour;
        [Required]
        internal int _maxHour;
        internal HourConditionDAL(int minHour, int maxHour, bool negative) : base(negative)
        {
            _minHour = minHour;
            _maxHour = maxHour;
        }
    }
    internal class DayOnWeekConditionDAL : AtomicConditionDAL
    {
        [Required]
        internal DayOfWeek _day;
        internal DayOnWeekConditionDAL(string day, bool negative) : base(negative)
        {
            _day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), day);
        }
    }
    internal class AndCompositionDAL : ComposedConditionDAL
    {
        internal AndCompositionDAL(List<ConditionDAL> conditionList, bool negative) : base(conditionList, negative)
        {
        }
    }
    internal class OrCompositionDAL : ComposedConditionDAL
    {
        internal OrCompositionDAL(List<ConditionDAL> conditionList, bool negative) : base(conditionList, negative)
        {
        }
    }
    internal class XorCompositionDAL : ComposedConditionDAL
    {
        internal XorCompositionDAL(List<ConditionDAL> conditionList, bool negative) : base(conditionList, negative)
        {
        }
    }
}