using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWebProject
{
    public class Response<T> : Response
    {
        public readonly T Value;
        internal Response(Exception e) : base(e) { }
        internal Response(T value) : base()
        {
            this.Value = value;
        }
        internal Response(T value, Exception e) : base(e)
        {
            this.Value = value;
        }
    }
}
