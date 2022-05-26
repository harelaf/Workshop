using MarketProject.Service;
using MarketProject.Service.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcceptanceTest
{
    [TestClass]
    public class TestGetAndAnswerStoreMessages
    {
        MarketAPI marketAPI = new MarketAPI();
        DateTime dob = new DateTime(2001, 7, 30);
        string storeName = "Krusty Krab";
        string username_founder = "SpongeBob SquarePants";
        string username_reviewer = "Mr. Depp";
        string guest_token;
        string registered_token_founder;
        string registered_token_reviewer;
        string title = "ThankYou Note";
        string messageBody = "very helpful customer service workers and great products.";


        [TestInitialize]
        public void setup()
        {
            guest_token = marketAPI.EnterSystem().Value;
            registered_token_founder = marketAPI.EnterSystem().Value;
            registered_token_reviewer = marketAPI.EnterSystem().Value;
            marketAPI.Register(registered_token_founder, username_founder, "123456789", dob);
            marketAPI.Register(registered_token_reviewer, username_reviewer, "123456789", dob);
            registered_token_founder = marketAPI.Login(registered_token_founder, username_founder, "123456789").Value;
            registered_token_reviewer = marketAPI.Login(registered_token_reviewer, username_reviewer, "123456789").Value;
            marketAPI.OpenNewStore(registered_token_founder, storeName);
        }

        [TestMethod]
        public void happy_reviewerSendsMessageAndTheMessageArrivesAsExpected()
        {
            Response response = marketAPI.SendMessageToStore(registered_token_reviewer, storeName, title, messageBody, 8);
            Response<Queue<MessageToStoreDTO>> response2 = marketAPI.GetStoreMessages(registered_token_founder, storeName);
            if (!response2.ErrorOccured)
            {
                MessageToStoreDTO messageObj = response2.Value.Dequeue();
                Assert.AreEqual(messageObj.Title, title);
                Assert.AreEqual(messageObj.StoreName, storeName);
                Assert.AreEqual(messageObj.SenderUsername, username_reviewer);
                Assert.AreEqual(messageObj.Message, messageBody);
            }
            else
            {
                Assert.Fail(response2.ErrorMessage);
            }
        }

        [TestMethod]
        public void sad_reviewerSendsMessageAndTheRequestsStoreMessages()
        {
            Response response = marketAPI.SendMessageToStore(registered_token_reviewer, storeName, title, messageBody, 9);
            Response<Queue<MessageToStoreDTO>> response2 = marketAPI.GetStoreMessages(registered_token_reviewer, storeName);
            Assert.IsTrue(response2.ErrorOccured);
        }

        [TestMethod]
        public void happy_reviewerSendMessageAndTheFounderReply()
        {
            String answerBody = "weare here for you 24/5.5";
            Response response = marketAPI.SendMessageToStore(registered_token_reviewer, storeName, title, messageBody, 80);
            Response<Queue<MessageToStoreDTO>> response2 = marketAPI.GetStoreMessages(registered_token_founder, storeName);
            if (!response2.ErrorOccured)
            {
                MessageToStoreDTO messageObj = response2.Value.Dequeue();
                Response response3 = marketAPI.AnswerStoreMesseage(registered_token_founder, messageObj.SenderUsername,80, storeName, answerBody);
                Assert.IsFalse(response3.ErrorOccured);
                Response<ICollection<MessageToStoreDTO>> res = marketAPI.GetRegisterAnsweredStoreMessages(registered_token_reviewer);
                if (!res.ErrorOccured)
                {
                    ICollection<MessageToStoreDTO> messages = res.Value;
                    MessageToStoreDTO message = messages.First();
                    Assert.AreEqual(title, message.Title);
                    Assert.AreEqual(messageBody, message.Message);
                    Assert.AreEqual(answerBody, message.Reply);
                }
            }
            else
            {
                Assert.Fail(response2.ErrorMessage);
            }
        }

        //[TestMethod]
        //public void sad_()
        //{
        //    
        //}
    }
}
