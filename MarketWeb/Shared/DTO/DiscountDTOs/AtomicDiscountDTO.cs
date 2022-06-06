using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Shared.DTO
{
    public interface AtomicDiscountDTO : IDiscountDTO
    {
        public int ObjType { get; set; }
    }
}
