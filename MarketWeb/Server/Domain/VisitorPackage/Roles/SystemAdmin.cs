﻿using System;
using System.Collections.Generic;
using System.Text;
using MarketWeb.Shared;


namespace MarketWeb.Server.Domain
{
    public class SystemAdmin : SystemRole
    {
        private IDictionary<int, Complaint> _receivedComplaints = new Dictionary<int, Complaint>();
        public IDictionary<int, Complaint> ReceivedComplaints => _receivedComplaints;
        public SystemAdmin(string Username) : base(getOps(), Username, null) {}
        public SystemAdmin(string Username, ISet<Operation> op, IDictionary<int, Complaint> receivedComplaints) : base(op, Username, null) 
        {
            _receivedComplaints = receivedComplaints;
        }
        private static ISet<Operation> getOps()
        {
            ISet<Operation> roles = new HashSet<Operation>();
            roles.Add(Operation.PERMENENT_CLOSE_STORE);
            roles.Add(Operation.CANCEL_SUBSCRIPTION);
            roles.Add(Operation.RECEIVE_AND_REPLY_ADMIN_MESSAGE);
            roles.Add(Operation.SYSTEM_STATISTICS);
            roles.Add(Operation.STORE_HISTORY_INFO);
            roles.Add(Operation.STORE_INFORMATION);
            return roles;
        }

        public void ReceiveComplaint(Complaint complaint)
        {
            _receivedComplaints.Add(complaint.ID, complaint);
        }

        public void ReplyToComplaint(int complaintID, String reply)
        {
            Complaint complaint;
            if (!_receivedComplaints.TryGetValue(complaintID, out complaint))
                throw new Exception($"No complaint with the ID {complaintID}.");
            complaint.Reply(reply);
        }
    }
}
