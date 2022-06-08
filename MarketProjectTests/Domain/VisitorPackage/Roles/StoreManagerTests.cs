using MarketWeb.Server.Domain;
using MarketWeb.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.Tests
{
    [TestClass]
    internal class StoreManagerTests
    {
        private SystemRole sr;
        private string storeName;
        private string appointer;

        [TestInitialize]
        public void setup()
        {
            storeName = "storeName";
            appointer = "someAppointer";
            sr = new StoreManager("Username", storeName, appointer);
        }

        [TestMethod]
        public void hasAccess_storeManager_returnstrue()
        {
            //act
            bool actual = sr.hasAccess(storeName, Operation.STORE_HISTORY_INFO) &&
                sr.hasAccess(storeName, Operation.RECEIVE_AND_REPLY_STORE_MESSAGE);
            //assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void hasAccess_storeManager_returnsfalse()
        {
            //act
            bool actual = sr.hasAccess(storeName, Operation.DEFINE_CONCISTENCY_CONSTRAINT) ||
                sr.hasAccess(storeName, Operation.MANAGE_INVENTORY);
            //assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void grantPermission_optionalOperation_returnstrue()
        {
            //arrange
            Operation op = Operation.DEFINE_CONCISTENCY_CONSTRAINT;
            bool part1 = sr.grantPermission(op, storeName, appointer);
            //act
            bool part2 = sr.hasAccess(storeName, op);
            //assert
            Assert.IsTrue(part1);
            Assert.IsTrue(part2);
        }

        [TestMethod]
        public void grantPermission_nonOptionalOperation_returnsfalse()
        {
            //arrange
            Operation op = Operation.APPOINT_OWNER;
            //act
            Assert.ThrowsException<Exception>(() => sr.grantPermission(op, storeName, appointer));
            bool part2 = sr.hasAccess(storeName, op);
            //assert
            Assert.IsFalse(part2);
        }

        public void denyPermission_grantAndDeny_returnstrue()
        {
            //arrange
            Operation op = Operation.DEFINE_CONCISTENCY_CONSTRAINT;
            //act
            try
            {
                bool part1 = sr.grantPermission(op, storeName, appointer) && sr.denyPermission(op, storeName, appointer);
                bool part2 = sr.hasAccess(storeName, op);
                //assert
                Assert.IsTrue(part1);
                Assert.IsFalse(part2);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        
        public void denyPermission_nonGrantedOp_returnstrue()
        {
            //arrange
            Operation op = Operation.DEFINE_CONCISTENCY_CONSTRAINT;
            //act
            try
            {
                bool part1 = sr.denyPermission(op, storeName, appointer);
                bool part2 = sr.hasAccess(storeName, op);
                //assert
                Assert.IsTrue(part1);
                Assert.IsFalse(part2);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            
        }

        [TestMethod]
        public void denyPermission_nonOptionalOperation_returnsfalse()
        {
            //arrange
            Operation op = Operation.APPOINT_OWNER;
            //act
            Assert.ThrowsException<Exception>(() => sr.denyPermission(op, storeName, appointer));
            bool part2 = sr.hasAccess(storeName, op);
            //assert
            Assert.IsFalse(part2);
        }

        [TestMethod]
        public void grantPermission_notByAppointer_returnsFalse()
        {
            //arrange
            Operation op = Operation.DEFINE_CONCISTENCY_CONSTRAINT;
            //act
            Assert.ThrowsException<Exception>(() => sr.grantPermission(op, storeName, appointer + "123"));
            bool part2 = sr.hasAccess(storeName, op);
            Assert.IsFalse(part2);
        }

        [TestMethod]
        public void denyPermission_notByAppointer_returnsFalse()
        {
            //arrange
            Operation op = Operation.DEFINE_CONCISTENCY_CONSTRAINT;
            //act
            bool part1 = sr.grantPermission(op, storeName, appointer);
            Assert.IsTrue(part1);
            Assert.ThrowsException<Exception>(() => sr.denyPermission(op, storeName, appointer + "123"));
            bool part3 = sr.hasAccess(storeName, op);
            Assert.IsTrue(part3);
        }
    }
}
