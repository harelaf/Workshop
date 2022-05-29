﻿using System.Collections.Generic;

namespace MarketWeb.Shared.DTO
{
    public class DiscountDetailsDTO
    {
        private int _amount;
        public int Amount => _amount;
        private List<AtomicDiscountDTO> _discountList;
        public List<AtomicDiscountDTO> DiscountList => _discountList;
        private double _actualPrice;
        public double ActualPrice => _actualPrice;
        public DiscountDetailsDTO(int amount, List<AtomicDiscountDTO> disList, double actualPrice)
        {
            _amount = amount;
            _discountList = disList;
            _actualPrice = actualPrice;
        }
    }
}