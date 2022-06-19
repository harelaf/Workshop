using Microsoft.VisualStudio.TestTools.UnitTesting;
using MarketWeb.Server.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.AspNetCore.SignalR;
using System.Dynamic;
using MarketWeb.Shared.DTO;

namespace MarketWeb.Server.Service.Tests
{
    [TestClass()]
    public class NotificationHubTests
    {
        // https://stackoverflow.com/questions/57577371/unit-testing-net-core-2-2-signalr-hub-issue-with-hubcontext
        [TestMethod()]
        public async Task SendNotification_Valid_Succeeds()
        {
            string authToken = "TestToken";
            string connectionId = "TestConnectionId";
            NotifyMessageDTO notifyMessageDTO = new NotifyMessageDTO("TestStore", "TestTitle", "Yo! This is the unit test.", "TestReceiver", 0);

            Mock<IHubClients> mockClients = new Mock<IHubClients>();
            Mock<IClientProxy> mockClientProxy = new Mock<IClientProxy>();
            Mock<IHubContext<NotificationHub>> hubContext = new Mock<IHubContext<NotificationHub>>();
            Mock<IConnectedUsers> mockConnectedUsers = new Mock<IConnectedUsers>();

            hubContext.Setup(x => x.Clients).Returns(() => mockClients.Object);
            mockClients.Setup(clients => clients.Client(connectionId)).Returns(mockClientProxy.Object);
            mockConnectedUsers.Setup(connectedUsers => connectedUsers.GetConnectionId(authToken)).Returns(connectionId);

            NotificationHub hub = new NotificationHub(hubContext.Object, mockConnectedUsers.Object);
            await hub.SendNotification(authToken, notifyMessageDTO);

            mockClients.Verify(clients => clients.Client(connectionId), Times.Once);
        }


    }
}