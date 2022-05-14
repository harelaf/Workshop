using MarketProject.Domain.PurchasePackage.DiscountPackage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Service.DTO
{ 
    public class HourConditionDTO : IConditionDTO
    {
        private int _minHour;
        private int _maxHour;
        private bool _negative;

        public int MinHour => _minHour;
        public int MaxHour => _maxHour;
        public bool Negative => _negative;
        public HourConditionDTO(int minHour, int maxHour, bool negative)
        {
            _maxHour = maxHour;
            _minHour = minHour;
            _negative = negative;
        }
        public DiscountCondition ConvertMe(dtoConditionConverter converter)
        {
            return converter.convertConcrete(this);
        }
    }
}
