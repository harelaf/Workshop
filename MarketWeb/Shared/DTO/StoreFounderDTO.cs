﻿
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Shared.DTO
{
    public class StoreFounderDTO
    {
        //private ISet<Operation> _operations;
        //public ISet<Operation> Operations => _operations;

        private String _storeName;
        public String StoreName => _storeName;

        private String _Username;
        public String Username => _Username;

        public StoreFounderDTO()
        {
            _storeName = "store1";
            _Username = "joe mama";
        }

        public StoreFounderDTO(string storeName, string username)
        {
            _storeName = storeName;
            _Username = username;
        }
    }
}