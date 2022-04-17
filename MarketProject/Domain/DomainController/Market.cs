using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    class Market
    {
        private StoreManagement _storeManagement;
        private UserManagement _userManagement;
        private History _history;

        public Market()
        {
            _storeManagement = new StoreManagement();
            _userManagement = new UserManagement();
            _history = new History();
        }

        public String GetStoreInformation(String storeName)
        {
            return _storeManagement.GetStoreInformation(storeName);
        }
    }
}
