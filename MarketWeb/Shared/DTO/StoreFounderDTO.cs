using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Shared.DTO
{
    public class StoreFounderDTO
    {
        private String _storeName;
        public String StoreName => _storeName;

        private String _username;
        public String Username => _username;

        public StoreFounderDTO(String username, String storeName)
        {
            _username = username;
            _storeName = storeName;
        }
    }
}
