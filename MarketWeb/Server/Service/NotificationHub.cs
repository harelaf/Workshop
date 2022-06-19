using MarketWeb.Shared.DTO;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarketWeb.Server.Service
{
    public interface IConnectedUsers
    {
        public void ChangeToken(string oldToken, string newToken);

        public string GetConnectionId(string authToken);
        public void AddConnection(string authToken, string connectionId);
        public void RemoveConnection(string authToken);
    }
    public class ConnectedUsers : IConnectedUsers
    {
        public IDictionary<string,string> TokenToConnectionId = new Dictionary<string,string>();

        public void ChangeToken(string oldToken, string newToken)
        {
            if (!TokenToConnectionId.ContainsKey(oldToken))
            {
                oldToken = "Bearer " + oldToken;
            }
            if (TokenToConnectionId.ContainsKey(oldToken))
            {
                string connectionId = this.TokenToConnectionId[oldToken];
                this.TokenToConnectionId.Remove(oldToken);
                this.TokenToConnectionId.Add(newToken, connectionId);
            }

        }

        public string GetConnectionId(string authToken)
        {
            return this.TokenToConnectionId[authToken]; 
        }
        
        public void AddConnection(string authToken, string connectionId)
        {
            if (TokenToConnectionId.ContainsKey(authToken))
                TokenToConnectionId[authToken] = connectionId;
            else
                this.TokenToConnectionId.Add(authToken, connectionId);
        }

        public void RemoveConnection(string authToken)
        {
            TokenToConnectionId.Remove(authToken);
        }
    }

    public class NotificationHub : Hub
    {
        protected IHubContext<NotificationHub> _hubContext;
        public IConnectedUsers _connectedUsers;

        public NotificationHub(IHubContext<NotificationHub> hubContext, IConnectedUsers connectedUsers)
        {
            _hubContext = hubContext;
            _connectedUsers = connectedUsers;
        }

        public async Task SendNotification(string authToken, NotifyMessageDTO notification)
        {
            await _hubContext.Clients.Client(_connectedUsers.GetConnectionId(authToken)).SendAsync("ReceiveNotification", notification);
        }
        
        public override Task OnConnectedAsync()
        {
            _connectedUsers.AddConnection(Context.GetHttpContext().Request.Query["access_token"], Context.ConnectionId);
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            _connectedUsers.RemoveConnection(Context.GetHttpContext().Request.Query["access_token"]);
            return base.OnDisconnectedAsync(exception);
        }

        
    }
}
