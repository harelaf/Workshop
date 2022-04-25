using MarketProject.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Service.DTO
{
    public class MessageToUserDTO
    {
        private String _storeName;
        public String StoreName => _storeName;

        private String _username;
        public String Username => _username;


        public MessageToUserDTO(MessageToUser messageToUser)
        {
            _storeName = messageToUser.StoreName;
            _username = messageToUser.Username;
        }
    }
}
