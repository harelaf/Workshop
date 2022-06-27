using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Shared
{
    //[JsonConverter(typeof(ResponseConverter))]
    public class Response<T> : Response
    {
        public T Value;
        public Response(Exception e) : base(e) { }
        public Response() : base()
        {
            Value = default;
        }
        public Response(T value) : base()
        {
            this.Value = value;
        }
        public Response(T value, Exception e) : base(e)
        {
            this.Value = value;
        }

        public static implicit operator Response<T>(Response<Dictionary<string, string>> v)
        {
            throw new NotImplementedException();
        }
    }
}
