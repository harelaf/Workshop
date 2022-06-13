using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MarketWeb.Server.DataLayer
{
    public class NotifyMessageDAL
    {
        [Key]
        public int mid { get; set; }
        [Required]
        public String _storeName { get; set; }
        [Required]
        public String _title { get; set; }
        [Required]
        public String _message { get; set; }
        [Required]
        public String _receiverUsername { get; set; }

        public NotifyMessageDAL(string storeName, string title, string message, string receiverUsername)
        {
            _storeName = storeName;
            _title = title;
            _message = message;
            _receiverUsername = receiverUsername;
        }

        public NotifyMessageDAL(int mid, string storeName, string title, string message, string receiverUsername)
        {
            this.mid = mid;
            _storeName = storeName;
            _title = title;
            _message = message;
            _receiverUsername = receiverUsername;
        }

        public NotifyMessageDAL()
        {
            // ???
        }
    }
}
