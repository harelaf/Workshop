namespace MarketWeb.Shared.DTO
{
    public class HourConditionDTO : IConditionDTO
    {
        private int _minHour;
        private int _maxHour;
        private bool _negative;

        public int MinHour => _minHour;
        public int MaxHour => _maxHour;
        public bool Negative => _negative;
        public HourConditionDTO(int minHour, int maxHour, bool negative)
        {
            _maxHour = maxHour;
            _minHour = minHour;
            _negative = negative;
        }
    }
}
