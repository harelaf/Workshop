using MarketWeb.Shared.DTO;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarketWeb.Server.Service
{
    public static class ConnectedUser
    {
        public static IDictionary<string,string> TokenToConnectionId = new Dictionary<string,string>();

        public static void ChangeToken(string oldToken, string newToken)
        {
            if (!TokenToConnectionId.ContainsKey(oldToken))
            {
                oldToken = "Bearer " + oldToken;
            }
            if (TokenToConnectionId.ContainsKey(oldToken))
            {
                string connectionId = ConnectedUser.TokenToConnectionId[oldToken];
                ConnectedUser.TokenToConnectionId.Remove(oldToken);
                ConnectedUser.TokenToConnectionId.Add(newToken, connectionId);
            }

        }
    }

    public class NotificationHub : Hub
    {
        protected IHubContext<NotificationHub> _hubContext;

        public NotificationHub(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendNotification(string authToken, NotifyMessageDTO notification)
        {
            await _hubContext.Clients.Client(ConnectedUser.TokenToConnectionId[authToken]).SendAsync("ReceiveNotification", notification);
        }
        
        public override Task OnConnectedAsync()
        {
            if (ConnectedUser.TokenToConnectionId.ContainsKey(Context.GetHttpContext().Request.Query["access_token"]))
                ConnectedUser.TokenToConnectionId[Context.GetHttpContext().Request.Query["access_token"]] = Context.ConnectionId;
            else
                ConnectedUser.TokenToConnectionId.Add(Context.GetHttpContext().Request.Query["access_token"], Context.ConnectionId);
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            ConnectedUser.TokenToConnectionId.Remove(Context.GetHttpContext().Request.Query["access_token"]);
            return base.OnDisconnectedAsync(exception);
        }

        
    }
}
