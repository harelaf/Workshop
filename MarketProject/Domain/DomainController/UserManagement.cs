﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class UserManagement
    {
        // Dictionary mapping identifier to User
        private IDictionary<String,Registered> _registeredUsers;
        // Dictionary mapping authentication token to user identifier
        private IDictionary<String, String> _authTokens;
        private ICollection<Registered> _logedinUsers;
        private ICollection<Guest> _visitors_guests;

        public UserManagement() : this(new Dictionary<String, Registered>()) { }

        public UserManagement(IDictionary<String, Registered> registeredUsers)
        {
            _registeredUsers = registeredUsers;
            _logedinUsers = new List<Registered>();
            _visitors_guests = new List<Guest>();
            _authTokens = new Dictionary<String, String>();
        }

        // Currently returns whether successful or not (bound to change)
        public bool Register(String username, String password)
        {
            if (_registeredUsers.ContainsKey(username) ||
                !CheckValidUsername(username) ||
                !CheckValidPassword(password))
                // May want to move validations to Registered during creation
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

        public String Login(String username, String password)
        {
            String authToken = null;
            Registered registered = GetRegisteredUser(username);
            if (registered != null &&  // User with the username exists
                !_authTokens.Values.Contains(username) && // Not logged in currently
                registered.Login(password)) // Login details correct
            {
                authToken = UserManagement.GenerateToken();
                if (!_authTokens.TryAdd(authToken, username))
                { // Something went wrong, couldn't add.
                    authToken = null;
                }
            }
            return authToken;
        }

        // Currently just a random string, from https://www.educative.io/edpresso/how-to-generate-a-random-string-in-c-sharp
        private static String GenerateToken()
        {
            String token = "";
            int tokenLength = 60;

            StringBuilder str_build = new StringBuilder();
            Random random = new Random();

            char letter;

            for (int i = 0; i < tokenLength; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }

            token = str_build.ToString();

            return token;
        }

        public Registered GetRegisteredUser(String username)
        {
            Registered registered;
            _registeredUsers.TryGetValue(username,out registered);
            return registered;
        }



        public bool isUserAVisitor(String username)
        {
            if (GetVisitorUser(username) == null)
                return false;
            return true;
        }

        public User GetVisitorUser(String username)
        {
            User user = GetRegisteredUser(username);
            if (user == null)// user is'nt registered
            {
                foreach (Guest guest in _visitors_guests)
                {
                    if (guest.System_username == username)
                        user= guest;
                }
            }
            return user;
        }
        public void addItemToUserCart(String username, Store store, Item item, int amount)
        {
            User user = GetVisitorUser(username);
            user.addItemToCart(store, item, amount);
        }
        public int RemoveItemFromCart(String username, Item item, Store store)
        {
            User user = GetVisitorUser(username);
            return user.RemoveItemFromCart(item, store);
        }
    }
}
