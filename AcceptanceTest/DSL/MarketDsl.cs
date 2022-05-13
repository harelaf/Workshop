using AcceptanceTest.DSL.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcceptanceTest.DSL
{
    /// <summary>
    /// <para>
    /// MarketDsl (Market Domain Specific Language) acts as the gateway or interface between the tests written 
    /// in the problem language to the protocol drivers which actually interact with the system.
    /// </para>
    /// Its main job is parsing the paramaters for the methods and delegating to the wanted driver.
    /// </summary>
    public class MarketDsl
    {
        private readonly IMarketDriver _driver;

        public MarketDsl(IMarketDriver driver)
        {
            _driver=driver;
        }



        public void EnterSystem() { _driver.EnterSystem(); }
        public void ExitSystem() { _driver.ExitSystem(); }
        public void Register(string Username, string password , DateTime birthDate) { _driver.Register(Username, password, birthDate); }

        public void Login(string Username, string password) { _driver.Login(Username, password); }

        internal void RemoveRegisteredVisitor(string Username) { _driver.RemoveRegisteredVisitor(Username); }

        internal void Logout() { _driver.Logout(); }

        internal void AssertErrorMessageRecieved() { _driver.AssertErrorMessageRecieved(); }

        public void AssertNoError() { _driver.AssertNoError(); }

        internal void ChangePassword(string oldPassword, string newPassword) { _driver.ChangePassword(oldPassword, newPassword); }
    }
}
