using System;

namespace AcceptanceTest.DSL.Drivers
{
    /// <summary>
    /// <para>WebMarketDriver interacts with the system through the web.</para>
    /// Will likely be needed for advanced iterations.
    /// </summary>
    public class WebMarketDriver : IMarketDriver
    {
        public void AssertErrorMessageRecieved()
        {
            throw new NotImplementedException();
        }

        public void AssertNoError()
        {
            throw new NotImplementedException();
        }

        public void ChangePassword(string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public void EnterSystem()
        {
            throw new NotImplementedException();
        }

        public void ExitSystem()
        {
            throw new NotImplementedException();
        }

        public void Login(string Username, string password)
        {
            throw new NotImplementedException();
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        public void Register(string Username, string password, DateTime birthDate)
        {
            throw new NotImplementedException();
        }

        public void RemoveRegisteredVisitor(string Username)
        {
            throw new NotImplementedException();
        }
    }
}
