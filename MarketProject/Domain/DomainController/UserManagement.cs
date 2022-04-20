using System;
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

        public UserManagement() : this(new Dictionary<String, Registered>()) { }

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
        public String Login(String username, String password)
        {
            String authToken = null;
            Registered registered = GetRegisteredUser(username);
            if (registered != null &&  // User with the username exists
                !_loggedinUsersTokens.Values.Contains(registered) && // Not logged in currently
                registered.Login(password)) // Login details correct
            {
                authToken = UserManagement.GenerateToken();
                if (!_loggedinUsersTokens.TryAdd(authToken, registered))
                { // Something went wrong, couldn't add.
                    authToken = null;
                }
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
        /// </summary>
        /// <param name="authToken"> The token of the user to log out.</param>
        public void Logout(String authToken)
        {
            if (IsUserLoggedin(authToken)) 
            {
                _loggedinUsersTokens.Remove(authToken);
            }
            else
            {
                throw new Exception("User not logged in.");
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

        public void AddItemToUserCart(String userToken, Store store, Item item, int amount)
        {
            User user = GetVisitorUser(userToken);
            user.AddItemToCart(store, item, amount);
        }

        internal bool checkAccess(string appointerUsername, string storeName, Operation op)
        {
            //Registered user = GetRegisteredUser(appointerUsername);
            //if (user != null)
            //    return user.hasAccess(storeName, op);
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

        internal void AddRole(string Username, SystemRole role)
        {
            GetRegisteredUser(Username).AddRole(role);
        }
    }
}
