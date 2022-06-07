using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain
{
    public class Complaint
    {
        private readonly int _id;
        public int ID { get { return _id; } }
        internal Registered _complainer { get; set; }
        internal int _cartID { get; set; }
        public int CartID { get { return _cartID; } }
        internal String _message { get; set; }
        public String Message { get { return _message; } }
        internal String _response { get; set; }
        public String Response { get { return _response; } }
        public ComplaintStatus Status { get { return _response == null ? ComplaintStatus.Open : ComplaintStatus.Closed; } }
        public Complaint(int id, Registered complainer, int cartID, string message)
        {
            _id = id;
            _complainer = complainer;
            _cartID = cartID;
            _message = message;
            _response = null;
        }

        public Complaint(int id, Registered complainer, int cartID, string message, string response) : this(id, complainer, cartID, message)
        {
            _response = response;
        }

        public void Reply(String response)
        {
            _response = response;
        }

        public String GetComplainer()
        {
            return _complainer.Username;
        }
    }

    public enum ComplaintStatus
    {
        Open,
        Closed
    }
}
