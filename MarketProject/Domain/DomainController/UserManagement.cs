using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    class UserManagement
    {
        private ICollection<User> _users;
        private ICollection<Registered> _logedinUsers;
        private ICollection<Guest> _visitors_guests;

        public UserManagement()
        {
            _users = new List<User>();
            _logedinUsers = new List<Registered>();
            _visitors_guests = new List<Guest>();
        }
        public bool isUserAVisitor(String username)
        {
            if (GetVisitorUser(username) == null)
                return false;
            return true;
        }
        public Registered GetRegisteredUser(String username)
        {
            foreach (Registered registered in _logedinUsers)
            {
                if (registered.Username == username)
                    return registered;
            }
            return null;
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
    }
}
