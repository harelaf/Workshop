﻿using System.Collections.Generic;

namespace MarketProject.Service.DTO
{
    public class PlusDiscountDTO : IDiscountDTO
    {
        private List<IDiscountDTO> _discounts;
        private IConditionDTO _condition;

        public List<IDiscountDTO> Discounts => _discounts;
        public IConditionDTO Condition => _condition;
        public PlusDiscountDTO(List<IDiscountDTO> discounts, IConditionDTO condition)
        {
            _discounts = discounts;
            _condition = condition;
        }
    }
}
