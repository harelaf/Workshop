using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class UserManagement

    {
        /// <summary>
        /// Dictionary mapping username to User
        /// </summary>
        private IDictionary<String, Registered> _registeredUsers;
        /// <summary>
        /// Dictionary mapping tokens to logged in users.
        /// </summary>
        private IDictionary<String, Registered> _loggedinUsersTokens;
        /// <summary>
        /// Dictionary mapping tokens to guests.
        /// </summary>
        private IDictionary<String, Guest> _visitorsGuestsTokens;
        private SystemAdmin _currentAdmin;
        public SystemAdmin CurrentAdmin { get { return _currentAdmin; } set { _currentAdmin = value; } }
        private int _nextComplaintID = 1;



        // ===================================== CONSTRUCTORS =====================================

        public UserManagement() : this(new Dictionary<String, Registered>()) { }

        // TODO: There's GOT to be a better way to do these constructors.
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



        // ===================================== GETTERS =====================================

        /// <summary>
        /// Returns the Registered user identified by username.
        /// </summary>
        /// <param name="username"> The username of the user to return.</param>
        /// <returns> The Registered user identified by username.</returns>
        public Registered GetRegisteredUser(String username)
        {
            Registered registered;
            _registeredUsers.TryGetValue(username, out registered);
            return registered;
        }

        /// <summary>
        /// Returns the username associated with a token.
        /// </summary>
        /// <param name="token"> The token of the user to return.</param>
        /// <returns> The username associated with a token.</returns>
        public String GetRegisteredUsernameByToken(String token)
        {
            if (!IsUserLoggedin(token))
                throw new ArgumentException("No registered user with the given token.");
            return _loggedinUsersTokens[token].Username;
        }

        /// <summary>
        /// Returns the Registered associated with a token.
        /// </summary>
        /// <param name="token"> The token of the Registered to return.</param>
        /// <returns> The Registered associated with a token.</returns>
        public Registered GetRegisteredByToken(String token)
        {
            if (!IsUserLoggedin(token))
                throw new ArgumentException("No registered user with the given token.");
            return _loggedinUsersTokens[token];
        }

        /// <summary>
        /// <para>Returns the user associated with a token.</para>
        /// <para>If it is an auth token (for Registered) returns the registered.</para>
        /// <para>If it is a temp username (for guest) returns the guest.</para>
        /// </summary>
        /// <param name="token"> The token of the user to return.</param>
        /// <returns> The user associated with a token.</returns>
        public User GetVisitorUser(String userToken)
        {
            if (_loggedinUsersTokens.ContainsKey(userToken))
                return _loggedinUsersTokens[userToken];
            if (_visitorsGuestsTokens.ContainsKey(userToken))
                return _visitorsGuestsTokens[userToken];
            throw new Exception("No user with the given token.");
        }

        internal ShoppingCart GetUserShoppingCart(string userToken)
        {
            User user = GetVisitorUser(userToken);
            return user.ShoppingCart;
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

        private String GetLoggedInToken(String username)
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



        // ===================================== Req I.1 - RESTART SYSTEM =====================================

        /// <summary>
        /// <para> For Req I.1. </para>
        /// <para> Starts system with the given credentials setting the user as the current admin (if he has permission).</para>
        /// </summary>
        public void AdminStart(string adminUsername, string adminPassword)
        {
            Registered registered = GetRegisteredUser(adminUsername);
            CurrentAdmin = registered.GetAdminRole;
        }



        // ===================================== Req II.1.1 - ENTER SYSTEM =====================================

        /// <summary>
        /// <para> For Req II.1.2. </para>
        /// <para> Generates and gives a token for a guest to use in the system.</para>
        /// </summary>
        /// <returns> The token for the guest entering.</returns>
        public String EnterSystem()
        {
            String token = GenerateToken();
            _visitorsGuestsTokens.Add(token, new Guest(token));
            return token;
        }



        // ===================================== Req II.1.2 - EXIT SYSTEM =====================================

        /// <summary>
        /// <para> For Req II.1.2. </para>
        /// <para> Removes the user's up-to-date tokens to exit.</para>
        /// </summary>
        /// <param name="authToken"> The token of the user exiting.</param>
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
                throw new Exception("Exit failed: there is no user with the given token currently in the system.");
            }
        }



        // ===================================== Req II.1.3 - REGISTER =====================================

        /// <summary>
        /// <para> For Req II.1.3. </para>
        /// <para> If credentials are valid, register new user.</para>
        /// </summary>
        /// <param name="username"> The username of the user to log in.</param>
        /// <param name="password"> The password to check.</param>
        public void Register(String username, String password)
        {
            if (_registeredUsers.ContainsKey(username))
                throw new Exception($"Username {username} unavailable.");
            if (!CheckValidUsername(username))
                throw new Exception($"Username {username} invalid.");
            if (!CheckValidPassword(password))
                throw new Exception($"Password is invalid.");

            Registered newRegistered = new Registered(username, password);

            _registeredUsers.Add(username, newRegistered); ;
        }

        /// <summary>
        /// <para> For Req II.1.3. </para>
        /// <para> Checks if username meets requirements.</para>
        /// </summary>
        /// <param name="username"> The username to validate.</param>
        private bool CheckValidUsername(String username)
        {
            return (username != "");
        }

        /// <summary>
        /// <para> For Req II.1.3. </para>
        /// <para> Checks if password meets requirements.</para>
        /// </summary>
        /// <param name="password"> The password to validate.</param>
        private bool CheckValidPassword(String password)
        {
            return (password != "");
        }

        public bool IsRegistered(String username)
        {
            return _registeredUsers.ContainsKey(username);
        }



        // ===================================== Req II.1.4 - LOGIN =====================================

        /// <summary>
        /// <para> For Req II.1.4. </para>
        /// <para> If credentials are authenticated, log in user.</para>
        /// </summary>
        /// <param name="username"> The username of the user to log in.</param>
        /// <param name="password"> The password to check.</param>
        /// <returns> The authentication token the user should use with the system.</returns>
        public String Login(String curToken, String username, String password)
        {
            if (!IsUserAGuest(curToken))
                throw new Exception("Must enter system as a guest in order to login.");
            _visitorsGuestsTokens.Remove(curToken);
            Registered registered = GetRegisteredUser(username);
            if (registered != null && _loggedinUsersTokens.Values.Contains(registered))
                throw new Exception($"User: {username} is already logged in to the system.");
            if (registered == null ||  // User with the username doesn't exists
                    !registered.Login(password))// Login details incorrect
                throw new Exception("Username or password are incorrect.");

            String authToken = UserManagement.GenerateToken();
            if (!_loggedinUsersTokens.TryAdd(authToken, registered))
            { // Something went wrong, couldn't add.
                throw new Exception("Login failed.");
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



        // ===================================== Req II.3.1 - LOGOUT =====================================

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
                    throw new Exception("Logout failed: could not tranfer to guest mode. Please try again");
                _visitorsGuestsTokens.Add(guestToken, new Guest(guestToken));
                return guestToken;

            }
            else
            {
                throw new Exception("Logout failed: User not logged in.");
            }
        }

        /// <summary>
        /// <para> For Req II.3.1 , II.6.2. </para>
        /// <para> Logs out a user via username. Allows an admin to logout other users.</para>
        /// </summary>
        /// <param name="username"> The username of the user to log out.</param>
        private void LogoutByUsername(String username)
        {
            String authToken = GetLoggedInToken(username);
            if (authToken != null)
            {
                Logout(authToken);
            }
        }



        // ===================================== Req II.3.6 - FILE COMPLAINT =====================================

        /// <summary>
        /// <para> For Req II.3.6. </para>
        /// <para> Files a complaint to the current system admin.</para>
        /// </summary>
        /// <param name="authToken"> The token of the user filing the complaint. </param>
        /// <param name="cartID"> The cart ID relevant to the complaint. </param>
        /// <param name="message"> The message detailing the complaint. </param>
        public void FileComplaint(String authToken, int cartID, String message)
        {
            Registered registered = GetRegisteredByToken(authToken);
            Complaint complaint = new Complaint(_nextComplaintID, registered, cartID, message);
            _nextComplaintID++;
            registered.FileComplaint(complaint);
            CurrentAdmin.ReceiveComplaint(complaint);
        }



        // ===================================== Req II.3.8 - EDIT USER DETAILS =====================================

        /// <summary>
        /// <para> For Req II.3.8. </para>
        /// <para> Updates a user's password if given the correct previous password.</para>
        /// </summary>
        /// <param name="authToken"> The authenticating token of the user changing the password.</param>
        /// <param name="oldPassword"> The user's current password. </param>
        /// <param name="newPassword"> The new updated password. </param>
        public void EditUserPassword(String authToken, String oldPassword, String newPassword)
        {
            Registered registered = GetRegisteredByToken(authToken);
            if (!CheckValidPassword(newPassword))
                throw new Exception($"Password is invalid.");
            registered.UpdatePassword(oldPassword, newPassword);
        }



        // ===================================== Req II.6.2 - REMOVE REGISTERED =====================================

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



        // ===================================== Req II.6.3 - REPLY TO COMPLAINT =====================================

        /// <summary>
        /// <para> For Req II.6.3. </para>
        /// <para> System admin replies to a complaint he received.</para>
        /// </summary>
        /// <param name="authToken"> The authorisation token of the system admin.</param>
        /// <param name="complaintID"> The ID of the complaint. </param>
        /// <param name="reply"> The response to the complaint. </param>
        public void ReplyToComplaint(String authToken, int complaintID, String reply)
        {
            Registered admin = GetRegisteredByToken(authToken);
            SystemAdmin adminRole = admin.GetAdminRole;
            if (adminRole == null)
                throw new Exception("User is not an admin.");
            adminRole.ReplyToComplaint(complaintID, reply);
        }



        // ===================================== CART OPERATIONS =====================================

        public void AddItemToUserCart(String userToken, Store store, Item item, int amount)
        {
            User user = GetVisitorUser(userToken);
            user.AddItemToCart(store, item, amount);
        }

        public int RemoveItemFromCart(String userToken, Item item, Store store)
        {
            User user = GetVisitorUser(userToken);
            return user.RemoveItemFromCart(item, store);
        }

        public void UpdateItemInUserCart(String userToken, Store store, Item item, int newQuantity)
        {
            if (newQuantity <= 0)
                throw new ArgumentOutOfRangeException("Cannot update quantity of item to non-positive amount.");
            User user = GetVisitorUser(userToken);
            user.UpdateItemInCart(store, item, newQuantity);
        }

        internal int GetUpdatingQuantityDifference(string userToken, Item item, Store store, int newQuantity)
        {
            User user = GetVisitorUser(userToken);
            int old_quantity = user.GetQuantityOfItemInCart(store, item);
            return newQuantity - old_quantity;
        }

        public ShoppingCart PurchaseMyCart(String userToken, String address, String city, String country, String zip, String purchaserName)
        {
            User user = GetVisitorUser(userToken);
            if (user.ShoppingCart.isCartEmpty())
                throw new Exception("Cannot purchase an empty cart.");
            return user.PurchaseMyCart(address, city, country, zip, purchaserName);
        }



        // ===================================== ROLE OPERATIONS =====================================

        internal bool CheckAccess(string appointerUsername, string storeName, Operation op)
        {
            Registered user = GetRegisteredUser(appointerUsername);
            if (user != null)
                return user.hasAccess(storeName, op);
            return false;
        }

        public void AddRole(string Username, SystemRole role)
        {
            Registered user = GetRegisteredUser(Username);
            if (user != null)
                user.AddRole(role);
        }

        public void RemoveRole(string Username, string storeName)
        {
            Registered user = GetRegisteredUser(Username);
            if (user != null)
                user.RemoveRole(storeName);
        }



        // ===================================== MESSAGE OPERATIONS =====================================

        public void SendMessageToRegistered(String storeName, String usernameReciever, String title, String message)
        {
            MessageToUser messageToUser = new MessageToUser(usernameReciever, storeName);
            Registered reciever = GetRegisteredUser(usernameReciever);
            reciever.SendMessage(messageToUser);
        }

        public void RemoveManagerPermission(String appointer, String managerUsername, String storeName, Operation op)
        {
            GetRegisteredUser(managerUsername).RemoveManagerPermission(appointer, storeName, op);
        }

        public void AddManagerPermission(String appointer, String managerUsername, String storeName, Operation op)
        {
            GetRegisteredUser(managerUsername).AddManagerPermission(appointer, storeName, op);
        }

        internal void AppointSystemAdmin(string adminUserName)
        {
            if (!IsRegistered(adminUserName))
                throw new Exception("this user is not registered.");
            GetRegisteredUser(adminUserName).AddRole(new SystemAdmin(adminUserName));
        }
    }
}
