using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MarketWeb.Server.DataLayer
{
    public class AdminMessageToRegisteredDAL
    {
        [Key]
        public int mid { get; set; }
        [Required]
        public String _senderUsername { get; set; }
        [Required]
        public String _title { get; set; }
        [Required]
        public String _message { get; set; }

        public AdminMessageToRegisteredDAL(int mid, string receiverUsername, string senderUsername, string title, string message)
        {
            this.mid = mid;
            _senderUsername = senderUsername;
            _title = title;
            _message = message;
        }

        public AdminMessageToRegisteredDAL(string receiverUsername, string senderUsername, string title, string message)
        {
            _senderUsername = senderUsername;
            _title = title;
            _message = message;
        }

        public AdminMessageToRegisteredDAL()
        {
            // ???
        }
    }
}
