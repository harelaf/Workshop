using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    class Rating
    {
        private Dictionary<String, Tuple<int, String>> _ratings; //<userId:String, <rating:int, review:String>>

        public Rating()
        {
            _ratings = new Dictionary<string, Tuple<int, string>>();
        }
    }
}
