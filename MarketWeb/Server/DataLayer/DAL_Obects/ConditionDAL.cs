using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Server.DataLayer
{
    public class ConditionDAL
    {
        [Key]
        public int _id { get; set; }
        [Required]
        public bool _negative;
        protected ConditionDAL(bool negative)
        {
            _negative = negative;
        }
        protected ConditionDAL()
        {
            // ???
        }
    }
    public class ComposedConditionDAL : ConditionDAL
    {
        public List<ConditionDAL> _conditionList { get; set; }
        protected ComposedConditionDAL(List<ConditionDAL> conditionList, bool negative) : base(negative)
        {
            if (conditionList == null)
                _conditionList = new List<ConditionDAL>();
            else _conditionList = conditionList;
        }
        protected ComposedConditionDAL()
        {
            // ???
        }
    }
    public class AtomicConditionDAL : ConditionDAL
    {
        protected AtomicConditionDAL(bool negative) : base(negative)
        {
        }
        protected AtomicConditionDAL()
        {
            // ???
        }
    }
    public class SearchablePriceableDAL : AtomicConditionDAL
    {
        [Required]
        public string _keyWord { get; set; }
        [Required] 
        public int _minValue { get; set; }
        [Required] 
        public int _maxValue { get; set; }

        protected SearchablePriceableDAL(string keyWord, int minValue, int maxValue, bool negative) : base(negative)
        {
            _keyWord = keyWord;
            _minValue = minValue;
            _maxValue = maxValue;
        }

        protected SearchablePriceableDAL()
        {
            // ???
        }
    }
    public class SearchItemConditionDAL : SearchablePriceableDAL
    {
        public SearchItemConditionDAL(string keyWord, int minValue, int maxValue, bool negative) : base(keyWord, minValue, maxValue, negative)
        {
        }
        public SearchItemConditionDAL()
        {
            // ???
        }
    }
    public class SearchCategoryConditionDAL : SearchablePriceableDAL
    {
        public SearchCategoryConditionDAL(string keyWord, int minValue, int maxValue, bool negative) : base(keyWord, minValue, maxValue, negative)
        {
        }
        public SearchCategoryConditionDAL()
        {
            // ???
        }
    }
    public class PriceableConditionDAL : SearchablePriceableDAL
    {
        public PriceableConditionDAL(int minValue, int maxValue, bool negative) : base("", minValue, maxValue, negative)
        {
        }
        public PriceableConditionDAL()
        {
            // ???
        }
    }
    public class HourConditionDAL : AtomicConditionDAL
    {
        [Required]
        public int _minHour { get; set; }
        [Required]
        public int _maxHour { get; set; }
        public HourConditionDAL(int minHour, int maxHour, bool negative) : base(negative)
        {
            _minHour = minHour;
            _maxHour = maxHour;
        }
        public HourConditionDAL()
        {
            // ???
        }
    }
    public class DayOnWeekConditionDAL : AtomicConditionDAL
    {
        [Required]
        public DayOfWeek _day { get; set; }
        public DayOnWeekConditionDAL(string day, bool negative) : base(negative)
        {
            _day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), day);
        }
        public DayOnWeekConditionDAL()
        {
            // ???
        }
    }
    public class AndCompositionDAL : ComposedConditionDAL
    {
        public AndCompositionDAL(List<ConditionDAL> conditionList, bool negative) : base(conditionList, negative)
        {
        }
        public AndCompositionDAL()
        {
            // ???
        }
    }
    public class OrCompositionDAL : ComposedConditionDAL
    {
        public OrCompositionDAL(List<ConditionDAL> conditionList, bool negative) : base(conditionList, negative)
        {
        }
        public OrCompositionDAL()
        {
            // ???
        }
    }
    public class XorCompositionDAL : ComposedConditionDAL
    {
        public XorCompositionDAL(List<ConditionDAL> conditionList, bool negative) : base(conditionList, negative)
        {
        }
        public XorCompositionDAL()
        {
            // ???
        }
    }
}