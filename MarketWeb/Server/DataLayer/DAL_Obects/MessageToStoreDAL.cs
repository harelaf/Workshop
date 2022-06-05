using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MarketWeb.Server.DataLayer
{
    public class MessageToStoreDAL
    {
        [Key]
        internal int mid { get; set; }
        [Required]
        internal String _storeName { get; set; }
        [Required]
        internal String _senderUsername { get; set; }
        [Required]
        internal String _message { get; set; }
        [Required]
        internal String _title { get; set; }
        internal string _reply { get; set; }
        internal string _replierFromStore { get; set; }


        public MessageToStoreDAL(string storeName, string senderUsername, string message, string title)
        {
            _storeName = storeName;
            _senderUsername = senderUsername;
            _message = message;
            _title = title;
        }
    }
}
