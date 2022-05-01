using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy.AtomicDiscountConditions
{
    public class HourCondition : DateTimeCondition
    {
        protected int _minHour, _maxHour;
        
        public HourCondition(int minHour, int maxHour) : base()
        {
            _minHour = minHour;
            _maxHour = maxHour;
        }
        public override bool Check()
        {
            if(_minHour < 0)
            {
                if(_maxHour < 0)
                {
                    return false;
                }
                return _now.Hour <= _maxHour;
            }
            else if(_maxHour < 0)
            {
                return _now.Hour >= _minHour;
            }
            return _now.Hour <= _maxHour && _now.Hour >= _minHour;
        }
    }
}
