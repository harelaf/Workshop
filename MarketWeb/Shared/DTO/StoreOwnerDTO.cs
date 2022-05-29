
using System;
using System.Collections.Generic;

namespace MarketWeb.Shared.DTO
{
    public class StoreOwnerDTO
    {
        private ISet<Operation> _operations;
        public ISet<Operation> Operations => _operations;

        private String _storeName;
        public String StoreName => _storeName;

        private String _username;
        public String Username => _username;

        private String _appointer;
        public String Appointer => _appointer;

        public StoreOwnerDTO(ISet<Operation> operations, string username, string storeName, string appointer)
        {
            _operations = operations;
            _storeName = storeName;
            _username = username;
            _appointer = appointer;
        }
    }
}
