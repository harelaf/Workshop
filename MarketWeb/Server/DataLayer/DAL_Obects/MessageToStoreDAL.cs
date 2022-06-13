using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MarketWeb.Server.DataLayer
{
    public class MessageToStoreDAL
    {
        [Key]
        public int mid { get; set; }
        [Required]
        public String _senderUsername { get; set; }
        [Required]
        public String _message { get; set; }
        [Required]
        public String _title { get; set; }
        public string _reply { get; set; }
        public string _replierFromStore { get; set; }

        public MessageToStoreDAL(int mid, string senderUsername, string message, string title, string reply, string replierFromStore)
        {
            this.mid = mid;
            _senderUsername = senderUsername;
            _message = message;
            _title = title;
            _reply = reply;
            _replierFromStore = replierFromStore;
        }

        public MessageToStoreDAL(string senderUsername, string message, string title)
        {
            _senderUsername = senderUsername;
            _message = message;
            _title = title;
        }

        public MessageToStoreDAL()
        {
            // ???
        }
    }
}
