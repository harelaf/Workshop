using System;
using System.Collections.Generic;

namespace MarketProject.Domain
{
    public class Registered : Visitor
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ICollection<AdminMessageToRegistered> _adminMessages;
        public ICollection<AdminMessageToRegistered> AdminMessages => _adminMessages;

        private ICollection<NotifyMessage> _notifications;
        public ICollection<NotifyMessage> Notifcations => _notifications;

        private ICollection<MessageToStore> _repliedMessages;
        public ICollection<MessageToStore> messageToStores => _repliedMessages;


        private String _username;
        public String Username=> _username;
        /// <summary>
        /// Password is salted and hashed!
        /// </summary>
        private String _password;
        private String _salt;
        public String Salt => _salt;

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
            _adminMessages = new List<AdminMessageToRegistered>();
            _notifications = new List<NotifyMessage>();
            _repliedMessages = new List<MessageToStore>();  
            _username = Username;
            SetPassword(password);
            _roles = new List<SystemRole>();
        }
        public void SendMessage(AdminMessageToRegistered message)
        {
            _adminMessages.Add(message);
        }

        internal bool hasAccess(String storeName, Operation op)
        {
            String errorMessage;
            foreach (SystemRole role in _roles)
            {
                if (role.hasAccess(storeName, op))
                    return true;
            }
            errorMessage = "no permission for this operation.";
            LogErrorMessage("hasAccess", errorMessage);
            throw new Exception(errorMessage);
        }

        public void AddRole(SystemRole role)
        {
            String errorMessage;
            if (hasRoleInStore(role.StoreName))
            {
                errorMessage = "already has this role.";
                LogErrorMessage("AddRole", errorMessage);
                throw new UnauthorizedAccessException(errorMessage);
            }
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
            return HashManager.CompareCleartextToHash(password, _salt, _password);
        }
        public bool RemoveRole(string storeName)
        {
            foreach(SystemRole role in _roles)
                if(role.StoreName == storeName)
                    return _roles.Remove(role);
            return false;
        }

        private void SetPassword(string password)
        {
            _salt = HashManager.GenerateSalt();
            _password = HashManager.Hash(password, Salt);
        }

        internal void UpdatePassword(string oldPassword, string newPassword)
        {
            String errorMessage;
            if (Login(oldPassword))
            {
                SetPassword(newPassword);
            }
            else
            {
                errorMessage = "Wrong password.";
                LogErrorMessage("UpdatePassword", errorMessage);
                throw new Exception(errorMessage);
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

        private void LogErrorMessage(String functionName, String message)
        {
            log.Error($"Exception thrown in Registered.{functionName}. Cause: {message}.");
        }

        internal void SendStoreMessageReplyment(MessageToStore msg, string replier, string reply)
        {
            msg.AnswerMsg(reply, replier);
            _repliedMessages.Add(msg);  
        }

        internal void SendNotificationMsg(string storeName, string title, string message)
        {
            _notifications.Add(new NotifyMessage(storeName, title, message, _username));
        }
    }
}
