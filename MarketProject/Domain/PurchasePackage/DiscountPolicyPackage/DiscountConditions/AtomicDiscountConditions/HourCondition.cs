﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    public class HourCondition : AtomicDiscountCondition
    {
        private int _minHour, _maxHour;
        public int MinHour
        {
            get { return _minHour; }
            private set
            {
                if (!IsInRange(value, 0, 24))
                    throw new ArgumentException("hour argument must be inside 0-24 range");
                _minHour = value;
            }
        }
        public int MaxHour
        {
            get { return _maxHour; }
            private set
            {
                if (!IsInRange(value, 0, 24))
                    throw new ArgumentException("hour argument must be inside 0-24 range");
                _maxHour = value;
            }
        }
        
        public HourCondition(int minHour, int maxHour, bool negative) : base(negative)
        {
            MaxHour = minHour;
            MinHour = maxHour;
        }
        public override bool Check(ISearchablePriceable searchablePriceable)
        {
            if(MaxHour < MinHour) // Example: (min - 14, max - 0) => check if current hour is *not* in range 0-14
                return !IsInRange(DateTime.Now.Hour, MaxHour, MinHour);
            else
                return IsInRange(DateTime.Now.Hour, MinHour, MaxHour);
        }
    }
}
