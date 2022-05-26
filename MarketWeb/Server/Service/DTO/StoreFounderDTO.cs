using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Service.DTO
{
    public class StoreFounderDTO
    {
        private String _storeName;
        public String StoreName => _storeName;

        private String _Username;
        public String Username => _Username;

        public StoreFounderDTO(String username, String storeName)
        {
            _Username = username;
            _storeName = storeName;
        }
    }
}
