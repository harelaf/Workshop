using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Shared
{
    public class Response<T> : Response
    {
        public readonly T Value;
        public Response(Exception e) : base(e) { }
        public Response(T value) : base()
        {
            this.Value = value;
        }
        public Response(T value, Exception e) : base(e)
        {
            this.Value = value;
        }
    }
}
