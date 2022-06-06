using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Shared.DTO
{
    public class NumericDiscountDTO : AtomicDiscountDTO
    {
        private double _priceToSubtract;
        private IConditionDTO _condition;
        private DateTime _expiration;

        public double PriceToSubtract => _priceToSubtract;
        public IConditionDTO Condition => _condition;
        public DateTime Expiration => _expiration;
        public int ObjType { get => 4; set { return; } }
        public NumericDiscountDTO(double priceToSubtract, IConditionDTO condition, DateTime expiration)
        {
            _priceToSubtract = priceToSubtract;
            _condition = condition;
            _expiration = expiration;
        }
    }
}
