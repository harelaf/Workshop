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

        private String _title;
        public String Title => _title;
        
        private String _message;
        public String Message => _message;


        public MessageToRegisteredDTO(AdminMessageToRegistered MessageToRegistered)
        {
            _storeName = MessageToRegistered.StoreName;
            _username = MessageToRegistered.ReceiverUsername;
            _title = MessageToRegistered.Title;
            _message = MessageToRegistered.Message;
        }
    }
}
