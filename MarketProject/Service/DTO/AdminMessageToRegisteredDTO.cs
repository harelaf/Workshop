using MarketProject.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Service.DTO
{
    public class AdminMessageToRegisteredDTO
    {
        private String _receiverUsername;
        public String ReceiverUsername => _receiverUsername;

        private String _senderUsername;
        public String senderUsername => _senderUsername;

        private String _title;
        public String Title => _title;

        private String _message;
        public String Message => _message;

        public AdminMessageToRegisteredDTO(AdminMessageToRegistered adminMessageToRegistered)
        {
            _receiverUsername = adminMessageToRegistered.ReceiverUsername;
            _senderUsername = adminMessageToRegistered.SenderUsername;
            _title = adminMessageToRegistered.Title;
            _message = adminMessageToRegistered.Message;
        }
    }
}
