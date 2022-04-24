using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcceptanceTest.DSL.Drivers
{
    /// <summary>
    /// <para>
    /// IMarketDriver is a protocol driver - it translates the language of problem
    /// domain to interactions with the system. </para>
    /// <para>
    /// It is the only part of the acceptance test infrastructure the understands 
    /// "HOW" things are done in the system.</para>
    /// </summary>
    public interface IMarketDriver
    {
        void EnterSystem();
        void ExitSystem();
        void Register(string username, string password);

        void Login(string username, string password);
        void Logout();
        void AssertNoError();
        void AssertErrorMessageRecieved();
        void RemoveRegisteredUser(string username);
    }
}
