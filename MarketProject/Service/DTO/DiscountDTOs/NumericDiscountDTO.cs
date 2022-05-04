using MarketProject.Domain;
using MarketProject.Domain.PurchasePackage.DiscountPackage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Service.DTO
{
    public class NumericDiscountDTO : IDiscountDTO
    {
        private double _priceToSubtract;
        private IConditionDTO _condition;
        private DateTime _expiration;

        public double PriceToSubtract => _priceToSubtract;
        public IConditionDTO Condition => _condition;
        public DateTime Expiration => _expiration;
        public NumericDiscountDTO(double priceToSubtract, IConditionDTO condition, DateTime expiration)
        {
            _priceToSubtract = priceToSubtract;
            _condition = condition;
            _expiration = expiration;
        }
        public Discount ConvertMe(dtoDiscountConverter converter)
        {
            return converter.ConvertConcrete(this);
        }
    }
}
