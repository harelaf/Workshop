using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Shared
{
    public class Response
    {
        public readonly string ErrorMessage;
        public bool ErrorOccured { get => ErrorMessage != null; }
        public Response() { }
        public Response(Exception e)
        {
            this.ErrorMessage = e.Message;
        }
    }
}
