using System;

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
        void Register(string Username, string password, DateTime birthDate);

        void Login(string Username, string password);
        void Logout();
        void AssertNoError();
        void AssertErrorMessageRecieved();
        void RemoveRegisteredVisitor(string Username);
        void ChangePassword(string oldPassword, string newPassword);
    }
}
