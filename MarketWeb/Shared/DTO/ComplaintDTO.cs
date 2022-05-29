using System;

namespace MarketWeb.Shared.DTO
{
    public class ComplaintDTO
    {
        private int _id;
        private int _cartID;
        private String _complainerUsername;
        private String _message;
        private String _response;

        public int ID { get { return _id; } set { _id = value; } }
        public int cartID { get { return _cartID; } set { _cartID = value; } }
        public String ComplainerUsername { get { return _complainerUsername; } set { _complainerUsername = value; } }
        public String Message { get { return _message; } set { _message = value; } }
        public String Response { get { return _response; } set { _response = value; } }

        public ComplaintDTO(int ID, int cartID, String complainer, String message, String response)
        {
            _id = ID;
            _cartID = cartID;
            _complainerUsername = complainer;
            _message = message;
            _response = response;
        }
    }
}
