using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class Complaint
    {
        private readonly int _id;
        public int ID { get { return _id; } }
        private Registered _complainer;
        private int _cardID;
        private String _message;
        private String _response;
        private ComplaintStatus Status { get { return _response == null ? ComplaintStatus.Open : ComplaintStatus.Closed; } }
        public Complaint(int id, Registered complainer, int cardID, string message)
        {
            _id = id;
            _complainer=complainer;
            _cardID=cardID;
            _message=message;
            _response = null;
        }

        public void Reply(String response)
        {
            _response = response;
        }
    }

    public enum ComplaintStatus
    {
        Open,
        Closed
    }
}
