using System;

namespace MarketWeb.Server.Domain
{
    public class Complaint
    {
        private readonly int _id;
        public int ID { get { return _id; } }
        private Registered _complainer;
        private int _cartID;
        public int CartID { get { return _cartID; } }
        private String _message;
        public String Message { get { return _message; } }
        private String _response;
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
