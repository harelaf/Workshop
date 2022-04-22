﻿using MarketProject.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Service.DTO
{
    public class StoreFounderDTO
    {
        private ISet<Operation> _operations;
        public ISet<Operation> Operations => _operations;

        private String _storeName;
        public String StoreName => _storeName;

        private String _username;
        public String Username => _username;

        public StoreFounderDTO(StoreFounder storeFounder)
        {
            _operations = new HashSet<Operation>(storeFounder.operations);
            _storeName = storeFounder.StoreName;
            _username = storeFounder.UserName;
        }
    }
}
