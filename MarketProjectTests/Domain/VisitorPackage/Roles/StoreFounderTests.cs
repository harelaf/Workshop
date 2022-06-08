using MarketWeb.Server.Domain;
using MarketWeb.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.Tests
{
    [TestClass]
    internal class StoreFounderTests
    {
        private StoreFounder sf;
        private string storeName;
        private string appointer;

        [TestInitialize]
        public void setup()
        {
            storeName = "storeName";
            appointer = "someAppointer";
            sf = new StoreFounder("Username", storeName);
        }

        [TestMethod]
        public void hasAccess_ExpectedPermissions_returnstrue()
        {
            //act
            bool actual = sf.hasAccess(storeName, Operation.DEFINE_CONCISTENCY_CONSTRAINT) &&
                sf.hasAccess(storeName, Operation.CLOSE_STORE) &&
                sf.hasAccess(storeName, Operation.REOPEN_STORE);
            //assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void hasAccess_UnExpectedPermissions_returnsfalse()
        {
            //act
            bool actual = sf.hasAccess(storeName, Operation.PERMENENT_CLOSE_STORE) ||
                sf.hasAccess(storeName, Operation.CANCEL_SUBSCRIPTION);
            //assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void grantPermission_returnsfalse()
        {
            //arrange
            Operation op = Operation.CANCEL_SUBSCRIPTION;
            bool part1 = sf.grantPermission(op, storeName, appointer);
            //act
            bool part2 = sf.hasAccess(storeName, op);
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
            bool part1 = sf.denyPermission(op, storeName, appointer);
            bool part2 = sf.hasAccess(storeName, op);
            //assert
            Assert.IsFalse(part1);
            Assert.IsTrue(part2);
        }
    }
}
