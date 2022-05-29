using System.Collections.Generic;

namespace MarketProject.Service.DTO
{
    public class XorCompositionDTO : IConditionDTO
    {
        private bool _negative;
        private List<IConditionDTO> _conditions;

        public bool Negative => _negative;
        public List<IConditionDTO> Conditions => _conditions;
        public XorCompositionDTO(bool negative, List<IConditionDTO> conditions)
        {
            _negative = negative;
            _conditions = conditions;
        }
    }
}
