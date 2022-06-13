using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MarketWeb.Server.DataLayer
{
    public class ShoppingBasketDAL
    {
        [Key]
        public int sbId { get; set; }
        [Required]
        public StoreDAL _store { get; set; }
        public ICollection<BasketItemDAL> _items { get; set; }

        public ShoppingBasketDAL(StoreDAL store, IDictionary<ItemDAL, PurchaseDetailsDAL> items)
        {
            _store = store;
            _items = new List<BasketItemDAL>();
            foreach (KeyValuePair<ItemDAL, PurchaseDetailsDAL> i_p in items)
            {
                _items.Add(new BasketItemDAL(i_p.Key, i_p.Value));
            }
        }

        public IDictionary<ItemDAL, PurchaseDetailsDAL> ConvertToDictionary()
        {
            IDictionary<ItemDAL, PurchaseDetailsDAL> dic = new Dictionary<ItemDAL, PurchaseDetailsDAL>();
            foreach (BasketItemDAL item in _items)
            {
                dic[item.item] = item.purchaseDetails;
            }
            return dic;
        }

        public ShoppingBasketDAL()
        {
            _items = new List<BasketItemDAL>();
        }
    }
}
