using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Shared
{
    //[JsonConverter(typeof(ResponseConverter))]
    public class Response
    {
        public string ErrorMessage;
        public bool ErrorOccured { get => ErrorMessage != null; }
        public Response() {
            ErrorMessage = null; 
        }
        public Response(Exception e)
        {
            this.ErrorMessage = e.Message;
        }
    }
}
