using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Shared.DTO
{
    public class AllProductsDiscountDTO : IDiscountDTO
    {
        private IConditionDTO _condition;
        private double _percentage;
        private DateTime _expiration;

        public IConditionDTO Condition => _condition;
        public double Percentage => _percentage;
        public DateTime Expiration => _expiration;

        public AllProductsDiscountDTO(double percentage_to_subtract, IConditionDTO condition, DateTime expiration)
        {
            _percentage = percentage_to_subtract;
            _condition = condition;
            _expiration = expiration;
        }
    }
}
