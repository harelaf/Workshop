using MarketProject.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Service.DTO
{
    public class StoreOwnerDTO
    {
        private ISet<Operation> _operations;
        public ISet<Operation> Operations => _operations;

        private String _storeName;
        public String StoreName => _storeName;

        private String _Username;
        public String Username => _Username;

        private String _appointer;
        public String Appointer => _appointer;

        public StoreOwnerDTO(StoreOwner storeOwner)
        {
            _operations = new HashSet<Operation>(storeOwner.operations);
            _storeName = storeOwner.StoreName;
            _Username = storeOwner.Username;
            _appointer = storeOwner.Appointer;
        }
    }
}
