using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWebProject
{
    public class Response
    {
        public readonly string ErrorMessage;
        public bool ErrorOccured { get => ErrorMessage != null; }
        internal Response() { }
        internal Response(Exception e)
        {
            this.ErrorMessage = e.Message;
        }
    }
}
