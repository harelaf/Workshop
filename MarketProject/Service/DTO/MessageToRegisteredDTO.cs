using MarketProject.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Service.DTO
{
    public class MessageToRegisteredDTO
    {
        private String _storeName;
        public String StoreName => _storeName;

        private String _username;
        public String Username => _username;


        public MessageToRegisteredDTO(MessageToRegistered messageToUser)
        {
            _storeName = messageToUser.StoreName;
            _username = messageToUser.Username;
        }
    }
}
