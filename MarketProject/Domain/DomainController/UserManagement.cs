﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class UserManagement

    { // Dictionary mapping username to User
        private IDictionary<String,Registered> _registeredUsers;
        // Dictionary mapping tokens to loggedin users.
        private IDictionary<String, Registered> _loggedinUsersTokens;
        // Dictionary mapping tokens to guests.
        private IDictionary<String, Guest> _visitorsGuestsTokens;

        public UserManagement()
        {
            _registeredUsers = new Dictionary<String,Registered>();
            _loggedinUsersTokens = new Dictionary<String, Registered>();
            _visitorsGuestsTokens = new Dictionary<String, Guest>();
        }

        public UserManagement(IDictionary<String, Registered> registeredUsers)
        {
            _registeredUsers = registeredUsers;
            _loggedinUsersTokens = new Dictionary<String, Registered>();
            _visitorsGuestsTokens = new Dictionary<String, Guest>();
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

        public String GetRegisteredUsernameByToken(String token)
        {
            if(!IsUserLoggedin(token))
                throw new ArgumentException("there is no registered user with that token");
            return _loggedinUsersTokens[token].Username;
        }

        public bool IsUserLoggedin(String userToken)
        {
            return _loggedinUsersTokens.ContainsKey(userToken);
        }
        public bool IsUserAVisitor(String userToken)
        {
            if (_visitorsGuestsTokens.ContainsKey(userToken) || IsUserLoggedin(userToken))
                return true;
            return false;
        }

        public User GetVisitorUser(String userToken)
        {
            if(_loggedinUsersTokens.ContainsKey(userToken))
                return _loggedinUsersTokens[userToken];
            if (_visitorsGuestsTokens.ContainsKey(userToken))
                return _visitorsGuestsTokens[userToken];
            throw new Exception("there is no user with that token in system.");
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
            Registered user = GetRegisteredUser(appointerUsername);
            if (user != null)
                return user.hasAccess(storeName, op);
            return false;
        }

        public int RemoveItemFromCart(String username, Item item, Store store)
        {
            User user = GetVisitorUser(username);
            return user.RemoveItemFromCart(item, store);
        }

        public void UpdateItemInUserCart(String userToken, Store store, Item item, int newQuantity)
        {
            if (newQuantity <= 0)
                throw new ArgumentOutOfRangeException("cant update quantity of item to non-positive amount");
            User user = GetVisitorUser(userToken);
            user.UpdateItemInCart(store, item, newQuantity);
        }

        internal int GetUpdatingQuanitityDiffrence(string userToken, Item item, Store store, int newQuantity)
        {
            User user = GetVisitorUser(userToken);
            int old_quantity = user.GetQuantityOfItemInCart(store, item);
            return newQuantity - old_quantity;
        }
        public ShoppingCart PurchaceMyCart(String userToken, String address, String city, String country, String zip, String purchaserName)
        {
             User user = GetVisitorUser(userToken);
             if (user.ShoppingCart.isCartEmpty())
                throw new Exception("can't purchase an emptyCart");
             return user.PurchaseMyCart(address, city, country, zip, purchaserName);
        }

        internal ShoppingCart GetUserShoppingCart(string userToken)
        {
            User user = GetVisitorUser(userToken);
            if (user.ShoppingCart.isCartEmpty())
                throw new Exception("Your shopping cart is empty!");
            return user.ShoppingCart;
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
