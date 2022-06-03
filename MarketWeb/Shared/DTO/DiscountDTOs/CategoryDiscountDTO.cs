using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Shared.DTO
{
    [JsonConverter(typeof(CategoryConverter))]
    public class CategoryDiscountDTO : AtomicDiscountDTO
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
    }
}
