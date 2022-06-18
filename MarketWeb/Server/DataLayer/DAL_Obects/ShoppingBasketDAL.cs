using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MarketWeb.Server.DataLayer
{
    public class ShoppingBasketDAL
    {
        [Key]
        public int sbId { get; set; }
        [Required]
        [ForeignKey("StoreDAL")]
        public StoreDAL _store { get; set; }
        [Required]
        public ICollection<BasketItemDAL> _items { get; set; }

        public ShoppingBasketDAL(StoreDAL store, IDictionary<int, PurchaseDetailsDAL> items)
        {
            _store = store;
            _items = new List<BasketItemDAL>();
            foreach (KeyValuePair<int, PurchaseDetailsDAL> i_p in items)
            {
                _items.Add(new BasketItemDAL(i_p.Key, i_p.Value));
            }
        }

        public ShoppingBasketDAL(StoreDAL store, ICollection<BasketItemDAL> items)
        {
            _store = store;
            _items = items;
        }

        public IDictionary<int, PurchaseDetailsDAL> ConvertToDictionary()
        {
            IDictionary<int, PurchaseDetailsDAL> dic = new Dictionary<int, PurchaseDetailsDAL>();
            foreach (BasketItemDAL item in _items)
            {
                dic[item.itemID] = item.purchaseDetails;
            }
            return dic;
        }

        public ShoppingBasketDAL()
        {
            _items = new List<BasketItemDAL>();
        }
    }
}
