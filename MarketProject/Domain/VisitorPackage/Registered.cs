using System;
using System.Collections.Generic;

namespace MarketProject.Domain
{
    public class Registered : Visitor
    {
        private ICollection<MessageToVisitor> _messagesToUser;
        private String _Username;
        public String Username=> _Username;
        private String _password;
        private ICollection<SystemRole> _roles;
        public ICollection<SystemRole> Roles { get { return _roles; } }
        private IDictionary<int,Complaint> _filedComplaints = new Dictionary<int,Complaint>();
        public bool IsAdmin 
        { 
            get 
            { 
                foreach (SystemRole role in _roles)
                {
                    if (role.GetType() == typeof(SystemAdmin))
                    {
                        return true;
                    }
                }
                return false; 
            } 
        }
        public SystemAdmin GetAdminRole
        {
            get
            {
                foreach (SystemRole role in _roles)
                {
                    if (role.GetType() == typeof(SystemAdmin))
                    {
                        return (SystemAdmin)role;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// All the stores in which the user has some role in.
        /// </summary>
        public ICollection<String> StoresWithRoles 
        { 
            get 
            {
                ICollection<String> stores = new HashSet<String>();
                foreach (SystemRole role in _roles)
                {
                    String storeName = role.StoreName;
                    stores.Add(storeName);
                }
                return stores; 
            } 
        }

        public Registered(string Username, string password)
        {
            _messagesToUser = new List<MessageToVisitor>();
            _Username = Username;
            _password = password;
            _roles = new List<SystemRole>();
        }
        public void SendMessage(MessageToVisitor message)
        {
            _messagesToUser.Add(message);
        }

        internal bool hasAccess(String storeName, Operation op)
        {
            foreach (SystemRole role in _roles)
            {
                if (role.hasAccess(storeName, op))
                    return true;
            }
            return false;
        }

        public void AddRole(SystemRole role)
        {
            if (hasRoleInStore(role.StoreName))
                throw new UnauthorizedAccessException("already has this role.");
            _roles.Add(role);
        }

        /// <summary>
        /// checks whether this user has a role in the same store.
        /// if admin, the store name is null and the check is valid.
        /// </summary>
        /// <param name="storeName"></param>
        /// <returns>true if has a role in this store (or if storename is null and user isn't a SystemAdmin), false otherwise.</returns>
        public bool hasRoleInStore(String storeName)
        {
            return GetRole(storeName) != null;
        }


        /// <summary>
        /// <para> For Req II.1.4. </para>
        /// <para> Checks if the given password authorises login for this user.</para>
        /// </summary>
        /// <param name="password"> The password to check.</param>
        /// <returns> True if the password authorises login, false otherwise.</returns>
        public bool Login(String password)
        {
            return (_password == password);
        }
        public bool RemoveRole(string storeName)
        {
            foreach(SystemRole role in _roles)
                if(role.StoreName == storeName)
                    return _roles.Remove(role);
            return false;
        }

        internal void UpdatePassword(string oldPassword, string newPassword)
        {
            if (Login(oldPassword))
            {
                _password = newPassword;
            }
            else
            {
                throw new Exception("Wrong password.");
            }
        }

        public void FileComplaint(Complaint complaint)
        {
            _filedComplaints.Add(complaint.ID, complaint);
        }

        public void RemoveManagerPermission(String appointer, String storeName, Operation op)
        {
            SystemRole role = GetRole(storeName);
            role.denyPermission(op, storeName, appointer);
        }

        public void AddManagerPermission(String appointer, String storeName, Operation op)
        {
            SystemRole role = GetRole(storeName);
            role.grantPermission(op, storeName, appointer);
        }

        private SystemRole GetRole(String storeName)
        {
            foreach(SystemRole role in _roles)
                if(role.StoreName == storeName)
                    return role;
            return null;
        }
    }
}
