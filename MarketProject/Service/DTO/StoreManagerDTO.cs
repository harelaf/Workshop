using MarketProject.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Service.DTO
{
    public class StoreManagerDTO
    {
        private ISet<Operation> _operations;
        public ISet<Operation> Operations => _operations;

        private String _storeName;
        public String StoreName => _storeName;

        private String _Username;
        public String Username => _Username;

        private String _appointer;
        public String Appointer => _appointer;

        public StoreManagerDTO(StoreManager storeManager)
        {
            _operations = new HashSet<Operation>(storeManager.operations);
            _storeName = storeManager.StoreName;
            _Username = storeManager.Username;
            _appointer = storeManager.Appointer;
        }
    }
}
