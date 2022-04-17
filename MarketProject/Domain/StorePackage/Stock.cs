using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    class Stock
    {
        private IDictionary<String, int> _stock; //<ItemId:String, quantity:int>

        public Stock()
        {
            _stock = new Dictionary<String, int>();
        }
    }
}
