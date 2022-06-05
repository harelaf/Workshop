using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MarketWeb.Server.DataLayer
{
    public class NotifyMessageDAL
    {
        [Key]
        internal int mid { get; set; }
        [Required]
        internal String _storeName { get; set; }
        [Required]
        internal String _title { get; set; }
        [Required]
        internal String _message { get; set; }
        [Required]
        internal String _receiverUsername { get; set; }

        public NotifyMessageDAL(string storeName, string title, string message, string receiverUsername)
        {
            _storeName = storeName;
            _title = title;
            _message = message;
            _receiverUsername = receiverUsername;
        }
    }
}
