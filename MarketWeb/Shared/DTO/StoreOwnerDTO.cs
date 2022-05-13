using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Shared.DTO
{
    public class StoreOwnerDTO
    {
        //private ISet<Operation> _operations;
        //public ISet<Operation> Operations => _operations;

        private String _storeName;
        public String StoreName => _storeName;

        private String _Username;
        public String Username => _Username;

        private String _appointer;
        public String Appointer => _appointer;

        public StoreOwnerDTO(string storeName, string username, string appointer)
        {
            _storeName = storeName;
            _Username = username;
            _appointer = appointer;
        }
    }
}
