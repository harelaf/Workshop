using MarketProject.Domain;
using MarketProject.Domain.PurchasePackage.DiscountPackage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Service.DTO
{
    public class CategoryDiscountDTO : IDiscountDTO
    {
        private double _percentage_to_subtract;
        private String _category;
        private IConditionDTO _condition;
        private DateTime _expiration;

        public double Percentage_to_subtract => _percentage_to_subtract;
        public String Category => _category;
        public IConditionDTO Condition => _condition;
        public DateTime Expiration => _expiration;
        public CategoryDiscountDTO(double percentage_to_subtract, String category, IConditionDTO condition, DateTime expiration)
        {
            _percentage_to_subtract = percentage_to_subtract;
            _category = category;
            _condition = condition;
            _expiration = expiration;
        }
        public Discount ConvertMe(dtoDiscountConverter converter)
        {
            return converter.ConvertConcrete(this);
        }
    }
}
