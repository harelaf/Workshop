using System;
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
            if (registered != null && registered.Login(password))
            {
                authToken = UserManagement.GenerateToken();
                _authTokens.Add(authToken, username);
            }
            return authToken;
        }

        private static String GenerateToken()
        {
            String token = "";

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
