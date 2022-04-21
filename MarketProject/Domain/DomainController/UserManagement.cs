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
        private SystemAdmin _currentAdmin;
        public SystemAdmin CurrentAdmin
        {
            get
            {
                return _currentAdmin;
            }
            set
            {
                _currentAdmin = value;
            }
        }


        public UserManagement() : this(new Dictionary<String, Registered>()) { }

        /// <summary>
        /// <para> For Req I.1. </para>
        /// <para> Starts system with the given credentials setting the user as the current admin (if he has permission).</para>
        /// </summary>
        public void AdminStart(string adminUsername, string adminPassword)
        {
            Registered registered = GetRegisteredUser(adminUsername);
            CurrentAdmin = registered.GetAdminRole;
        }

        // TODO: Theres GOT to be a better way to do these constructors.
        public UserManagement(IDictionary<String, Registered> registeredUsers)
        {
            _registeredUsers = registeredUsers;
            _loggedinUsersTokens = new Dictionary<String, Registered>();
            _visitorsGuestsTokens = new Dictionary<String, Guest>();
        }

        public UserManagement(IDictionary<String, Registered> registeredUsers, IDictionary<String, Registered> loggedinUsersTokens)
        {
            _registeredUsers = registeredUsers;
            _loggedinUsersTokens = loggedinUsersTokens;
            _visitorsGuestsTokens = new Dictionary<String, Guest>();
        }

        public UserManagement(IDictionary<String, Registered> registeredUsers, IDictionary<String, Guest> visitorsGuestsTokens)
        {
            _registeredUsers = registeredUsers;
            _loggedinUsersTokens = new Dictionary<String, Registered>(); 
            _visitorsGuestsTokens = visitorsGuestsTokens;
        }

        public UserManagement(IDictionary<String, Registered> registeredUsers, IDictionary<String, Registered> loggedinUsersTokens, IDictionary<String, Guest> visitorsGuestsTokens)
        {
            _registeredUsers = registeredUsers;
            _loggedinUsersTokens = loggedinUsersTokens;
            _visitorsGuestsTokens = visitorsGuestsTokens;
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

        public bool IsRegistered(String username)
        {
            return _registeredUsers.ContainsKey(username);
        }

        /// <summary>
        /// <para> For Req II.1.4. </para>
        /// <para> If credentials are authenticated, log in user.</para>
        /// </summary>
        /// <param name="username"> The username of the user to log in.</param>
        /// <param name="password"> The password to check.</param>
        /// <returns> The authentication token the user should use with the system.</returns>
        public String Login(String curToken, String username, String password)
        {
            if (!IsUserAGuest(username))
                throw new Exception("you have to enter system as a guest in order to loggin.");
            _visitorsGuestsTokens.Remove(curToken);
            Registered registered = GetRegisteredUser(username);
            if (registered != null && _loggedinUsersTokens.Values.Contains(registered))
                throw new Exception($"user: {username} is allredy loggedin to system.");
            if (registered == null ||  // User with the username doesn't exists
                    !registered.Login(password))// Login details incorrect
                throw new Exception("username or password are incorrect!");
            
            String authToken = UserManagement.GenerateToken();
            if (!_loggedinUsersTokens.TryAdd(authToken, registered))
            { // Something went wrong, couldn't add.
                throw new Exception("loggin faild. please try again");
            }
            return authToken;
        }

        /// <summary>
        /// <para> For Req II.1.4. </para>
        /// <para> Generates a long random string.</para>
        /// <para> Currently just a random string, as shown in the How To <see href="https://www.educative.io/edpresso/how-to-generate-a-random-string-in-c-sharp">HERE</see> </para>
        /// </summary>
        /// <param name="username"> The username of the user to log in.</param>
        /// <param name="password"> The password to check.</param>
        /// <returns> The authentication token the user should use with the system.</returns>
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

        /// <summary>
        /// <para> For Req II.3.1. </para>
        /// <para> Log out user identified by authToken.</para>
        /// <returns>: string token of guest</returns>
        /// </summary>
        /// <param name="authToken"> The token of the user to log out.</param>
        public String Logout(String authToken)
        {
            if (IsUserLoggedin(authToken)) 
            {
                _loggedinUsersTokens.Remove(authToken);
                String guestToken = GenerateToken();
                if (guestToken == null)
                    throw new Exception("logout faild: couldent tranfer to guest mode. please try again");
                _visitorsGuestsTokens.Add(guestToken, new Guest(guestToken));
                return guestToken;

            }
            else
            {
                throw new Exception("logout faild: User not logged in.");
            }
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
            if (IsUserAGuest(userToken) || IsUserLoggedin(userToken))
                return true;
            return false;
        }
        public bool IsUserAGuest(String userToken)
        {
            return _visitorsGuestsTokens.ContainsKey(userToken);
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

        public void AddItemToUserCart(String userToken, Store store, Item item, int amount)
        {
            User user = GetVisitorUser(userToken);
            user.AddItemToCart(store, item, amount);
        }

        internal bool checkAccess(string appointerUsername, string storeName, Operation op)
        {
            Registered user = GetRegisteredUser(appointerUsername);
            if (user != null)
                return user.hasAccess(storeName, op);
            return false;
        }

        public int RemoveItemFromCart(String userToken, Item item, Store store)
        {
            User user = GetVisitorUser(userToken);
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
            Registered user = GetRegisteredUser(Username);
            if (user != null)
                user.AddRole(role);
        }

        public bool RemoveRole(string Username, string storeName)
        {
            Registered user = GetRegisteredUser(Username);
            if (user != null)
                user.RemoveRole(storeName);
        }

        /// <summary>
        /// <para> For Req II.6.2. </para>
        /// <para> Logs out if needed and removes a user from our system.</para>
        /// </summary>
        /// <param name="username"> The username of the user to log out.</param>
        public void RemoveRegisteredUser(string username)
        {
            if (!IsRegistered(username))
            {
                throw new Exception("No such registered user.");
            }

            LogoutByUsername(username);
            _registeredUsers.Remove(username);
        }

        /// <summary>
        /// <para> For Req II.6.2. </para>
        /// <para> Logs out a user via username. Allows an admin to logout other users.</para>
        /// </summary>
        /// <param name="username"> The username of the user to log out.</param>
        private void LogoutByUsername(String username)
        {
            String authToken = getLoggedInToken(username);
            if (authToken != null)
            {
                Logout(authToken);
            }
        }

        private String getLoggedInToken(String username)
        {
            foreach (KeyValuePair<String, Registered> pair in _loggedinUsersTokens)
            {
                if (_loggedinUsersTokens[pair.Key].Username == username)
                {
                    return pair.Key;
                }
            }
            return null;
        }

        public String enter()
        {
            String token = GenerateToken();
            _visitorsGuestsTokens.Add(token, new Guest(token));
            return token;
        }

        public void ExitSystem(string authToken)
        {
            if (IsUserLoggedin(authToken))
            {
                //_loggedinUsersTokens[authToken].saveCart(); ---> save cart in exit
                _loggedinUsersTokens.Remove(authToken);
            }
            else if (IsUserAGuest(authToken))
            {
                _visitorsGuestsTokens.Remove(authToken);
            }
            else
            {
                throw new Exception("Exit faild: there is no user with that tokem in system");
            }
        }

    }
}
