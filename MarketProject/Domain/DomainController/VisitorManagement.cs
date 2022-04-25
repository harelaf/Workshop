using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class VisitorManagement

    {
        /// <summary>
        /// Dictionary mapping Username to Visitor
        /// </summary>
        private IDictionary<String, Registered> _registeredVisitors;
        /// <summary>
        /// Dictionary mapping tokens to logged in Visitors.
        /// </summary>
        private IDictionary<String, Registered> _loggedinVisitorsTokens;
        /// <summary>
        /// Dictionary mapping tokens to guests.
        /// </summary>
        private IDictionary<String, Guest> _visitorsGuestsTokens;
        private SystemAdmin _currentAdmin;
        public SystemAdmin CurrentAdmin { get { return _currentAdmin; } set { _currentAdmin = value; } }
        private int _nextComplaintID = 1;
        private static readonly string DEFAULT_ADMIN_USERNAME = "admin";
        private static readonly string DEFAULT_ADMIN_PASSWORD = "admin";




        // ===================================== CONSTRUCTORS =====================================

        public VisitorManagement() : this(new Dictionary<String, Registered>()) { }

        // TODO: There's GOT to be a better way to do these constructors.
        public VisitorManagement(IDictionary<String, Registered> registeredVisitors)
        {
            _registeredVisitors = registeredVisitors;
            _loggedinVisitorsTokens = new Dictionary<String, Registered>();
            _visitorsGuestsTokens = new Dictionary<String, Guest>();

            Registered defaultAdmin = new Registered(DEFAULT_ADMIN_USERNAME, DEFAULT_ADMIN_PASSWORD);
            SystemAdmin defaultAdminRole = new SystemAdmin(DEFAULT_ADMIN_USERNAME);
            defaultAdmin.AddRole(defaultAdminRole);
            _registeredVisitors.Add(DEFAULT_ADMIN_USERNAME, defaultAdmin);
        }

        public VisitorManagement(IDictionary<String, Registered> registeredVisitors, IDictionary<String, Registered> loggedinVisitorsTokens)
        {
            _registeredVisitors = registeredVisitors;
            _loggedinVisitorsTokens = loggedinVisitorsTokens;
            _visitorsGuestsTokens = new Dictionary<String, Guest>();

            Registered defaultAdmin = new Registered(DEFAULT_ADMIN_USERNAME, DEFAULT_ADMIN_PASSWORD);
            SystemAdmin defaultAdminRole = new SystemAdmin(DEFAULT_ADMIN_USERNAME);
            defaultAdmin.AddRole(defaultAdminRole);
            _registeredVisitors.Add(DEFAULT_ADMIN_USERNAME, defaultAdmin);
        }

        public VisitorManagement(IDictionary<String, Registered> registeredVisitors, IDictionary<String, Guest> visitorsGuestsTokens)
        {
            _registeredVisitors = registeredVisitors;
            _loggedinVisitorsTokens = new Dictionary<String, Registered>();
            _visitorsGuestsTokens = visitorsGuestsTokens;

            Registered defaultAdmin = new Registered(DEFAULT_ADMIN_USERNAME, DEFAULT_ADMIN_PASSWORD);
            SystemAdmin defaultAdminRole = new SystemAdmin(DEFAULT_ADMIN_USERNAME);
            defaultAdmin.AddRole(defaultAdminRole);
            _registeredVisitors.Add(DEFAULT_ADMIN_USERNAME, defaultAdmin);
        }

        public VisitorManagement(IDictionary<String, Registered> registeredVisitors, IDictionary<String, Registered> loggedinVisitorsTokens, IDictionary<String, Guest> visitorsGuestsTokens)
        {
            _registeredVisitors = registeredVisitors;
            _loggedinVisitorsTokens = loggedinVisitorsTokens;
            _visitorsGuestsTokens = visitorsGuestsTokens;

            Registered defaultAdmin = new Registered(DEFAULT_ADMIN_USERNAME, DEFAULT_ADMIN_PASSWORD);
            SystemAdmin defaultAdminRole = new SystemAdmin(DEFAULT_ADMIN_USERNAME);
            defaultAdmin.AddRole(defaultAdminRole);
            _registeredVisitors.Add(DEFAULT_ADMIN_USERNAME, defaultAdmin);
        }



        // ===================================== GETTERS =====================================

        /// <summary>
        /// Returns the Registered Visitor identified by Username.
        /// </summary>
        /// <param name="Username"> The Username of the Visitor to return.</param>
        /// <returns> The Registered Visitor identified by Username.</returns>
        public Registered GetRegisteredVisitor(String Username)
        {
            Registered registered;
            _registeredVisitors.TryGetValue(Username, out registered);
            return registered;
        }

        /// <summary>
        /// Returns the Username associated with a token.
        /// </summary>
        /// <param name="token"> The token of the Visitor to return.</param>
        /// <returns> The Username associated with a token.</returns>
        public String GetRegisteredUsernameByToken(String token)
        {
            if (!IsVisitorLoggedin(token))
                throw new ArgumentException("No registered Visitor with the given token.");
            return _loggedinVisitorsTokens[token].Username;
        }

        /// <summary>
        /// Returns the Registered associated with a token.
        /// </summary>
        /// <param name="token"> The token of the Registered to return.</param>
        /// <returns> The Registered associated with a token.</returns>
        public Registered GetRegisteredByToken(String token)
        {
            if (!IsVisitorLoggedin(token))
                throw new ArgumentException("No registered Visitor with the given token.");
            return _loggedinVisitorsTokens[token];
        }

        /// <summary>
        /// <para>Returns the Visitor associated with a token.</para>
        /// <para>If it is an auth token (for Registered) returns the registered.</para>
        /// <para>If it is a temp Username (for guest) returns the guest.</para>
        /// </summary>
        /// <param name="token"> The token of the Visitor to return.</param>
        /// <returns> The Visitor associated with a token.</returns>
        public Visitor GetVisitorVisitor(String VisitorToken)
        {
            if (_loggedinVisitorsTokens.ContainsKey(VisitorToken))
                return _loggedinVisitorsTokens[VisitorToken];
            if (_visitorsGuestsTokens.ContainsKey(VisitorToken))
                return _visitorsGuestsTokens[VisitorToken];
            throw new Exception("No Visitor with the given token.");
        }

        internal ShoppingCart GetVisitorShoppingCart(string VisitorToken)
        {
            Visitor Visitor = GetVisitorVisitor(VisitorToken);
            return Visitor.ShoppingCart;
        }

        public bool IsVisitorLoggedin(String VisitorToken)
        {
            return _loggedinVisitorsTokens.ContainsKey(VisitorToken);
        }

        public bool IsVisitorAVisitor(String VisitorToken)
        {
            if (IsVisitorAGuest(VisitorToken) || IsVisitorLoggedin(VisitorToken))
                return true;
            return false;
        }

        public bool IsVisitorAGuest(String VisitorToken)
        {
            return _visitorsGuestsTokens.ContainsKey(VisitorToken);
        }

        private String GetLoggedInToken(String Username)
        {
            foreach (KeyValuePair<String, Registered> pair in _loggedinVisitorsTokens)
            {
                if (_loggedinVisitorsTokens[pair.Key].Username == Username)
                {
                    return pair.Key;
                }
            }
            return null;
        }



        // ===================================== Req I.1 - RESTART SYSTEM =====================================

        /// <summary>
        /// <para> For Req I.1. </para>
        /// <para> Starts system with the given credentials setting the Visitor as the current admin (if he has permission).</para>
        /// </summary>
        public void AdminStart(string adminUsername, string adminPassword)
        {
            Registered registered = GetRegisteredVisitor(adminUsername);
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
        /// <para> Removes the Visitor's up-to-date tokens to exit.</para>
        /// </summary>
        /// <param name="authToken"> The token of the Visitor exiting.</param>
        public void ExitSystem(string authToken)
        {
            if (IsVisitorLoggedin(authToken))
            {
                //_loggedinVisitorsTokens[authToken].saveCart(); ---> save cart in exit
                _loggedinVisitorsTokens.Remove(authToken);
            }
            else if (IsVisitorAGuest(authToken))
            {
                _visitorsGuestsTokens.Remove(authToken);
            }
            else
            {
                throw new Exception("Exit failed: there is no Visitor with the given token currently in the system.");
            }
        }



        // ===================================== Req II.1.3 - REGISTER =====================================

        /// <summary>
        /// <para> For Req II.1.3. </para>
        /// <para> If credentials are valid, register new Visitor.</para>
        /// </summary>
        /// <param name="Username"> The Username of the Visitor to log in.</param>
        /// <param name="password"> The password to check.</param>
        public void Register(String Username, String password)
        {
            if (_registeredVisitors.ContainsKey(Username))
                throw new Exception($"Username {Username} unavailable.");
            if (!CheckValidUsername(Username))
                throw new Exception($"Username {Username} invalid.");
            if (!CheckValidPassword(password))
                throw new Exception($"Password is invalid.");

            Registered newRegistered = new Registered(Username, password);

            _registeredVisitors.Add(Username, newRegistered); ;
        }

        /// <summary>
        /// <para> For Req II.1.3. </para>
        /// <para> Checks if Username meets requirements.</para>
        /// </summary>
        /// <param name="Username"> The Username to validate.</param>
        private bool CheckValidUsername(String Username)
        {
            return (Username != "");
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

        public bool IsRegistered(String Username)
        {
            return _registeredVisitors.ContainsKey(Username);
        }



        // ===================================== Req II.1.4 - LOGIN =====================================

        /// <summary>
        /// <para> For Req II.1.4. </para>
        /// <para> If credentials are authenticated, log in Visitor.</para>
        /// </summary>
        /// <param name="Username"> The Username of the Visitor to log in.</param>
        /// <param name="password"> The password to check.</param>
        /// <returns> The authentication token the Visitor should use with the system.</returns>
        public String Login(String curToken, String Username, String password)
        {
            if (!IsVisitorAGuest(curToken))
                throw new Exception("Must enter system as a guest in order to login.");
            _visitorsGuestsTokens.Remove(curToken);
            Registered registered = GetRegisteredVisitor(Username);
            if (registered != null && _loggedinVisitorsTokens.Values.Contains(registered))
                throw new Exception($"Visitor: {Username} is already logged in to the system.");
            if (registered == null ||  // Visitor with the Username doesn't exists
                    !registered.Login(password))// Login details incorrect
                throw new Exception("Username or password are incorrect.");

            String authToken = VisitorManagement.GenerateToken();
            if (!_loggedinVisitorsTokens.TryAdd(authToken, registered))
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
        /// <param name="Username"> The Username of the Visitor to log in.</param>
        /// <param name="password"> The password to check.</param>
        /// <returns> The authentication token the Visitor should use with the system.</returns>
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
        /// <para> Log out Visitor identified by authToken.</para>
        /// <returns>: string token of guest</returns>
        /// </summary>
        /// <param name="authToken"> The token of the Visitor to log out.</param>
        public String Logout(String authToken)
        {
            if (IsVisitorLoggedin(authToken))
            {
                _loggedinVisitorsTokens.Remove(authToken);
                String guestToken = GenerateToken();
                if (guestToken == null)
                    throw new Exception("Logout failed: could not tranfer to guest mode. Please try again");
                _visitorsGuestsTokens.Add(guestToken, new Guest(guestToken));
                return guestToken;

            }
            else
            {
                throw new Exception("Logout failed: Visitor not logged in.");
            }
        }

        /// <summary>
        /// <para> For Req II.3.1 , II.6.2. </para>
        /// <para> Logs out a Visitor via Username. Allows an admin to logout other Visitors.</para>
        /// </summary>
        /// <param name="Username"> The Username of the Visitor to log out.</param>
        private void LogoutByUsername(String Username)
        {
            String authToken = GetLoggedInToken(Username);
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
        /// <param name="authToken"> The token of the Visitor filing the complaint. </param>
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



        // ===================================== Req II.3.8 - EDIT Visitor DETAILS =====================================

        /// <summary>
        /// <para> For Req II.3.8. </para>
        /// <para> Updates a Visitor's password if given the correct previous password.</para>
        /// </summary>
        /// <param name="authToken"> The authenticating token of the Visitor changing the password.</param>
        /// <param name="oldPassword"> The Visitor's current password. </param>
        /// <param name="newPassword"> The new updated password. </param>
        public void EditVisitorPassword(String authToken, String oldPassword, String newPassword)
        {
            Registered registered = GetRegisteredByToken(authToken);
            if (!CheckValidPassword(newPassword))
                throw new Exception($"Password is invalid.");
            registered.UpdatePassword(oldPassword, newPassword);
        }



        // ===================================== Req II.6.2 - REMOVE REGISTERED =====================================

        /// <summary>
        /// <para> For Req II.6.2. </para>
        /// <para> Logs out if needed and removes a Visitor from our system.</para>
        /// </summary>
        /// <param name="Username"> The Username of the Visitor to log out.</param>
        public void RemoveRegisteredVisitor(string Username)
        {
            if (!IsRegistered(Username))
            {
                throw new Exception("No such registered Visitor.");
            }

            LogoutByUsername(Username);
            _registeredVisitors.Remove(Username);
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
                throw new Exception("Visitor is not an admin.");
            adminRole.ReplyToComplaint(complaintID, reply);
        }



        // ===================================== CART OPERATIONS =====================================

        public void AddItemToVisitorCart(String VisitorToken, Store store, Item item, int amount)
        {
            Visitor Visitor = GetVisitorVisitor(VisitorToken);
            Visitor.AddItemToCart(store, item, amount);
        }

        public int RemoveItemFromCart(String VisitorToken, Item item, Store store)
        {
            Visitor Visitor = GetVisitorVisitor(VisitorToken);
            return Visitor.RemoveItemFromCart(item, store);
        }

        public void UpdateItemInVisitorCart(String VisitorToken, Store store, Item item, int newQuantity)
        {
            if (newQuantity <= 0)
                throw new ArgumentOutOfRangeException("Cannot update quantity of item to non-positive amount.");
            Visitor Visitor = GetVisitorVisitor(VisitorToken);
            Visitor.UpdateItemInCart(store, item, newQuantity);
        }

        internal int GetUpdatingQuantityDifference(string VisitorToken, Item item, Store store, int newQuantity)
        {
            Visitor Visitor = GetVisitorVisitor(VisitorToken);
            int old_quantity = Visitor.GetQuantityOfItemInCart(store, item);
            return newQuantity - old_quantity;
        }

        public ShoppingCart PurchaseMyCart(String VisitorToken, String address, String city, String country, String zip, String purchaserName)
        {
            Visitor Visitor = GetVisitorVisitor(VisitorToken);
            if (Visitor.ShoppingCart.isCartEmpty())
                throw new Exception("Cannot purchase an empty cart.");
            return Visitor.PurchaseMyCart(address, city, country, zip, purchaserName);
        }



        // ===================================== ROLE OPERATIONS =====================================

        internal bool CheckAccess(string appointerUsername, string storeName, Operation op)
        {
            Registered Visitor = GetRegisteredVisitor(appointerUsername);
            if (Visitor != null)
                return Visitor.hasAccess(storeName, op);
            return false;
        }

        public void AddRole(string Username, SystemRole role)
        {
            Registered Visitor = GetRegisteredVisitor(Username);
            if (Visitor != null)
                Visitor.AddRole(role);
        }

        public void RemoveRole(string Username, string storeName)
        {
            Registered Visitor = GetRegisteredVisitor(Username);
            if (Visitor != null)
                Visitor.RemoveRole(storeName);
        }



        // ===================================== MESSAGE OPERATIONS =====================================

        public void SendMessageToRegistered(String storeName, String UsernameReciever, String title, String message)
        {
            MessageToRegistered messageToRegistered = new MessageToRegistered(UsernameReciever, storeName);
            Registered reciever = GetRegisteredVisitor(UsernameReciever);
            reciever.SendMessage(messageToRegistered);
        }

        public void RemoveManagerPermission(String appointer, String managerUsername, String storeName, Operation op)
        {
            GetRegisteredVisitor(managerUsername).RemoveManagerPermission(appointer, storeName, op);
        }

        public void AddManagerPermission(String appointer, String managerUsername, String storeName, Operation op)
        {
            GetRegisteredVisitor(managerUsername).AddManagerPermission(appointer, storeName, op);
        }

        internal void AppointSystemAdmin(string adminUsername)
        {
            if (!IsRegistered(adminUsername))
                throw new Exception("this Visitor is not registered.");
            GetRegisteredVisitor(adminUsername).AddRole(new SystemAdmin(adminUsername));
        }
    }
}
