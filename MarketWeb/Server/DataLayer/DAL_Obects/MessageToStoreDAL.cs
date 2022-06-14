using MarketWeb.Server.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MarketWeb.Server.DataLayer
{
    public enum StoreMessageStatus
    {
        Open,
        Closed
    }
    public class MessageToStoreDAL
    {
        [Key]
        public int mid { get; set; }
        [ForeignKey("RegisterdeDAL")]
        public String _senderUsername { get; set; }
        [ForeignKey("StoreDAL")]
        public string _storeName { get; set; }
        [Required]
        public String _message { get; set; }
        [Required]
        public String _title { get; set; }
        public string _reply { get; set; }
        public string _replierFromStore { get; set; }
        //[NotMapped]
        // StoreMessageStatus Status { get { return _reply == null ? StoreMessageStatus.Open : StoreMessageStatus.Closed; } }

        public StoreMessageStatus Status() { return _reply == null ? StoreMessageStatus.Open : StoreMessageStatus.Closed; }

        public MessageToStoreDAL(int mid, string senderUsername, string message, string title, string reply, string replierFromStore, string storename)
        {
            this.mid = mid;
            _senderUsername = senderUsername;
            _message = message;
            _title = title;
            _reply = reply;
            _replierFromStore = replierFromStore;
            _storeName = storename;
        }

        public MessageToStoreDAL(string senderUsername, string message, string title, string storename)
        {
            _senderUsername = senderUsername;
            _message = message;
            _title = title;
            _storeName=storename;
        }

        public MessageToStoreDAL()
        {
            // ???
        }
    }
}
