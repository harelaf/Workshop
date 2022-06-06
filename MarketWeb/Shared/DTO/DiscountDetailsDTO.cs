using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Shared.DTO
{
    //[JsonConverter(typeof(DetailsConverter))]
    public class DiscountDetailsDTO
    {
        private int _amount;
        public int Amount => _amount;
        //private List<AtomicDiscountDTO> _discountList;
        //public List<AtomicDiscountDTO> DiscountList => _discountList;
        private List<String> _discountList;
        public List<String> DiscountList => _discountList;
        private double _actualPrice;
        public double ActualPrice => _actualPrice;
        public DiscountDetailsDTO(int amount, List<String> disList, double actualPrice)
        {
            _amount = amount;
            if(disList == null)
                _discountList = new List<String>();
            else _discountList = disList;
            _actualPrice = actualPrice;
        }
    }
}
