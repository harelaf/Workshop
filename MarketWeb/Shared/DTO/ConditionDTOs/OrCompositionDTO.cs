using System.Collections.Generic;

namespace MarketWeb.Shared.DTO
{
    public class OrCompositionDTO : IConditionDTO
    {
        private bool _negative; 
        private List<IConditionDTO> _conditions;

        public bool Negative => _negative;
        public List<IConditionDTO> Conditions => _conditions;
        public OrCompositionDTO(bool negative, List<IConditionDTO> conditions)
        {
            _negative = negative;
            _conditions = conditions;
        }
    }
}
