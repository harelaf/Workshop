using System.Collections.Generic;

namespace MarketWeb.Shared.DTO
{
    public class MaxDiscountDTO : IDiscountDTO
    {
        private List<IDiscountDTO> _discounts;
        private IConditionDTO _condition;

        public List<IDiscountDTO> Discounts => _discounts;
        public IConditionDTO Condition => _condition;
        public MaxDiscountDTO(List<IDiscountDTO> discounts, IConditionDTO condition)
        {
            _discounts = discounts;
            _condition = condition;
        }
    }
}
