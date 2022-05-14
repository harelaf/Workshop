using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Shared.DTO
{
    public class ItemDiscountDTO : IDiscountDTO
    {
        private double _percentage_to_subtract;
        private String _itemName;
        private IConditionDTO _condition;
        private DateTime _expiration;

        public double PercentageToSubtract => _percentage_to_subtract;
        public String ItemName => _itemName;
        public IConditionDTO Condition => _condition;
        public DateTime Expiration => _expiration;
        public ItemDiscountDTO(double percentage_to_subtract, String itemName, IConditionDTO condition, DateTime expiration)
        {
            _percentage_to_subtract = percentage_to_subtract;
            _itemName = itemName;
            _condition = condition;
            _expiration = expiration;
        }
    }
}
