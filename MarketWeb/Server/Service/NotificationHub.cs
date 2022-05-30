using MarketWeb.Shared.DTO;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarketWeb.Server.Service
{
    public static class ConnectedUser
    {
        public static List<string> Ids = new List<string>();
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
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification);
        }

        public override Task OnConnectedAsync()
        {
            ConnectedUser.Ids.Add(Context.ConnectionId);
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            ConnectedUser.Ids.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
