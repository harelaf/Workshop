using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class UserManagement
    {
        // Dictionary mapping identifier to User
        private IDictionary<String,Registered> _registeredUsers;
        private ICollection<Registered> _logedinUsers;
        private ICollection<Guest> _visitors_guests;

        public UserManagement()
        {
            _registeredUsers = new Dictionary<String, Registered>();
            _logedinUsers = new List<Registered>();
            _visitors_guests = new List<Guest>();
        }

        // Currently returns whether successful or not (bound to change)
        public bool Register(String username, String password)
        {
            if (_registeredUsers.ContainsKey(username) ||
                !CheckValidUsername(username) ||
                !CheckValidPassword(password))
                // Username taken
            {
                return false;
            }
            // May want to add username and password validity checks
            Registered newRegistered = new Registered(username, password);

            _registeredUsers.Add(username, newRegistered);

            return true;
        }

        private bool CheckValidUsername(String username)
        {
            return (username != "");
        }

        private bool CheckValidPassword(String password)
        {
            return (password != "");
        }

        public bool IsRegistered(String username)
        {
            return _registeredUsers.ContainsKey(username);
        }

        public Registered GetRegisteredUser(String username)
        {
            Registered registered;
            _registeredUsers.TryGetValue(username, out registered);
            return registered;
        }

        public bool IsUserAVisitor(String username)
        {
            return GetVisitorUser(username) != null;
        }

        public User GetVisitorUser(String username)
        {
            User user = GetRegisteredUser(username);
            if (user == null)// user isn't registered
            {
                foreach (Guest guest in _visitors_guests)
                {
                    if (guest.System_username == username)
                        user= guest;
                }
            }
            return user;
        }

        public void SendMessageToRegisterd(String storeName, String usernameReciever, String title, String message)
        {
            MessageToUser messageToUser = new MessageToUser(usernameReciever, storeName);
            Registered reciever = GetRegisteredUser(usernameReciever);
            reciever.SendMessage(messageToUser);
        }

        public void AddItemToUserCart(String username, Store store, Item item, int amount)
        {
            User user = GetVisitorUser(username);
            user.AddItemToCart(store, item, amount);
        }

        internal bool checkAccess(string appointerUsername, string storeName, Operation op)
        {
            //Registered user = GetRegisteredUser(appointerUsername);
            //if (user != null)
            //    return user.hasAccess(storeName, op);
            return false;
        }

        public int RemoveItemFromCart(String username, Item item, Store store)
        {
            User user = GetVisitorUser(username);
            return user.RemoveItemFromCart(item, store);
        }

        public void UpdateItemInUserCart(String username, Store store, Item item, int newQuantity)
        {
            if (newQuantity <= 0)
                throw new ArgumentOutOfRangeException("cant update quantity of item to non-positive amount");
            User user = GetVisitorUser(username);
            user.UpdateItemInCart(store, item, newQuantity);
        }

        internal int GetUpdatingQuanitityDiffrence(string username, Item item, Store store, int newQuantity)
        {
            User user = GetVisitorUser(username);
            int old_quantity = user.GetQuantityOfItemInCart(store, item);
            return newQuantity - old_quantity;
        }
        public void PurchaceMyCart(String userToken)
        {

        }

        public void AddRole(string Username, SystemRole role)
        {
            GetRegisteredUser(Username).AddRole(role);
        }

        public bool RemoveRole(string ownerUsername, string storeName)
        {
            return GetRegisteredUser(ownerUsername).RemoveRole(storeName);
        }
    }
}
