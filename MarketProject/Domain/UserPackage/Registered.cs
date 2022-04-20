﻿using System;
using System.Collections.Generic;

namespace MarketProject.Domain
{
    public class Registered : User
    {
        private ICollection<MessageToUser> _messagesToUser;
        private String _username;
        public String Username=> _username;
        private String _password;
        private ICollection<SystemRole> _roles;

        public Registered(string username, string password)
        {
            _messagesToUser = new List<MessageToUser>();
            _username = username;
            _password = password;
        }
        public void SendMessage(MessageToUser message)
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

        internal void AddRole(SystemRole role)
        {
            if (!hasRoleInStore(role.StoreName))
                throw new UnauthorizedAccessException("already has this role.");
            _roles.Add(role);
        }

        /// <summary>
        /// checks whether this user has a role in the same store.
        /// if admin, the store name is null and the check is valid.
        /// </summary>
        /// <param name="storeName"></param>
        /// <returns>true if has a role in this store (or if storename is null and user isn't a SystemAdmin), false otherwise.</returns>
        internal bool hasRoleInStore(String storeName)
        {
            foreach (SystemRole role in _roles)
                if (role.StoreName == storeName)
                    return true;
            return false;
        }

        internal void RemoveManagerRole(string storeName)
        {
            throw new NotImplementedException();
        }
    }
}
