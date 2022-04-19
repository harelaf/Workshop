using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.Tests
{
    [TestClass]
    internal class SystemAdminTests
    {
        private SystemAdmin sa;
        private string storeName;
        
        [TestInitialize]
        public void setup()
        {
            sa = new SystemAdmin();
            storeName = null;
        }

        [TestMethod]
        public void hasAccess_ExpectedPermissions_returnstrue()
        {
            //act
            bool actual = sa.hasAccess(storeName, Operation.PERMENENT_CLOSE_STORE) &&
                sa.hasAccess(storeName, Operation.CANCEL_SUBSCRIPTION) &&
                sa.hasAccess(storeName, Operation.RECEIVE_AND_REPLY_ADMIN_MESSAGE) &&
                sa.hasAccess(storeName, Operation.SYSTEM_STATISTICS);
            //assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void hasAccess_UnExpectedPermissions_returnsfalse()
        {
            //act
            bool actual = sa.hasAccess(storeName, Operation.DEFINE_CONCISTENCY_CONSTRAINT) ||
                sa.hasAccess(storeName, Operation.CLOSE_STORE) ||
                sa.hasAccess(storeName, Operation.APPOINT_MANAGER);
            //assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void grantPermission_returnsfalse()
        {
            //arrange
            Operation op = Operation.APPOINT_OWNER;
            bool part1 = sa.grantPermission(op, storeName, "someAppointer");
            //act
            bool part2 = sa.hasAccess(storeName, op);
            //assert
            Assert.IsFalse(part1);
            Assert.IsFalse(part2);
        }

        [TestMethod]
        public void denyPermission_returnsfalse()
        {
            //arrange
            Operation op = Operation.CANCEL_SUBSCRIPTION;
            //act
            bool part1 = sa.denyPermission(op, storeName, "someAppointer");
            bool part2 = sa.hasAccess(storeName, op);
            //assert
            Assert.IsFalse(part1);
            Assert.IsTrue(part2);
        }
    }
}
