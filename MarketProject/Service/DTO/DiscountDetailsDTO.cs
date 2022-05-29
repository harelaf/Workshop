using System.Collections.Generic;

namespace MarketProject.Service.DTO
{
    public class DiscountDetailsDTO
    {
        private int _amount;
        public int Amount => _amount;
        private List<AtomicDiscountDTO> _discountList;
        public List<AtomicDiscountDTO> DiscountList => _discountList;
        public DiscountDetailsDTO(int amount, List<AtomicDiscountDTO> disList)
        {
            _amount = amount;
            _discountList = disList;
        }
    }
}
