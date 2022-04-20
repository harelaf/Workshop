using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.Tests
{
    [TestClass]
    internal class StoreOwnerTests
    {
        private StoreOwner so;
        private string storeName;
        private string appointer;

        [TestInitialize]
        public void setup()
        {
            storeName = "storeName";
            appointer = "someAppointer";
            so = new StoreOwner("userName", storeName, appointer);
        }

        [TestMethod]
        public void hasAccess_ExpectedPermissions_returnstrue()
        {
            //act
            bool actual = so.hasAccess(storeName, Operation.MANAGE_INVENTORY) &&
                so.hasAccess(storeName, Operation.CHANGE_SHOP_AND_DISCOUNT_POLICY) &&
                so.hasAccess(storeName, Operation.APPOINT_OWNER) &&
                so.hasAccess(storeName, Operation.REMOVE_OWNER) &&
                so.hasAccess(storeName, Operation.APPOINT_MANAGER) &&
                so.hasAccess(storeName, Operation.REMOVE_MANAGER) &&
                so.hasAccess(storeName, Operation.CHANGE_MANAGER_PREMISSIONS) &&
                so.hasAccess(storeName, Operation.STORE_WORKERS_INFO) &&
                so.hasAccess(storeName, Operation.RECEIVE_AND_REPLY_STORE_MESSAGE);
            //assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void hasAccess_UnExpectedPermissions_returnsfalse()
        {
            //act
            bool actual = so.hasAccess(storeName, Operation.DEFINE_CONCISTENCY_CONSTRAINT) ||
                so.hasAccess(storeName, Operation.CLOSE_STORE) ||
                so.hasAccess(storeName, Operation.SYSTEM_STATISTICS);
            //assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void grantPermission_returnsfalse()
        {
            //arrange
            Operation op = Operation.CANCEL_SUBSCRIPTION;
            bool part1 = so.grantPermission(op, storeName, appointer);
            //act
            bool part2 = so.hasAccess(storeName, op);
            //assert
            Assert.IsFalse(part1);
            Assert.IsFalse(part2);
        }

        [TestMethod]
        public void denyPermission_returnsfalse()
        {
            //arrange
            Operation op = Operation.APPOINT_OWNER;
            //act
            bool part1 = so.denyPermission(op, storeName, appointer);
            bool part2 = so.hasAccess(storeName, op);
            //assert
            Assert.IsFalse(part1);
            Assert.IsTrue(part2);
        }
    }
}
