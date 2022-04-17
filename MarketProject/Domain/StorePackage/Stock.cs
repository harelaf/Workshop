using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    class Stock
    {
        private IDictionary<Item, int> _stock; //<Item, quantity:int>

        public Stock()
        {
            _stock = new Dictionary<Item, int>();
        }

        public List<String> GetItemNames()
        {
            List<Item> keyList = new List<Item>(_stock.Keys);
            List<String> names = new List<String>();

            foreach (Item item in keyList)
            {
                names.Add(item.GetName());
            }

            return names;
        }
    }
}
