using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWebProject.DTO
{
    public class RegisteredDTO
    {
        private String _username;
        public String Username => _username;
        private ShoppingCartDTO _shoppingCart;
        public ShoppingCartDTO ShoppingCart => _shoppingCart;
        private List<Tuple<MessageToStoreDTO, MessageToRegisteredDTO>> _messages;
        public List<Tuple<MessageToStoreDTO, MessageToRegisteredDTO>> Messages => _messages;

        public RegisteredDTO()
        {
            _username = "user1";
            _shoppingCart = new ShoppingCartDTO();

            _messages = new List<Tuple<MessageToStoreDTO, MessageToRegisteredDTO>>();
            MessageToStoreDTO message = new MessageToStoreDTO("Joe Mama's Store", _username, "Joe Sucks", "Mama Sucks");
            MessageToRegisteredDTO answer = new MessageToRegisteredDTO("Re: Joe Mama's Store", "", "JOE MAMA", "HAHAHAHAH LOSER");
            _messages.Add(new Tuple<MessageToStoreDTO, MessageToRegisteredDTO>(message, answer));
            MessageToStoreDTO message2 = new MessageToStoreDTO("Walmart", _username, "I LOVE KFC", "PLEASE SELL MCDONALDS!!!");
            _messages.Add(new Tuple<MessageToStoreDTO, MessageToRegisteredDTO>(message2, null));

        }

        public String ToString()
        {
            String result = $"Visitor Name: {_username}\n";
            result += "Current Cart State:\n" + _shoppingCart.ToString();
            return result;
        }
        public int MessagesCount()
        {
            return _messages.Count;
        }
    }
}
