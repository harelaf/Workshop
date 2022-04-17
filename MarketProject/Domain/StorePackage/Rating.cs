using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    class Rating
    {
        private Dictionary<String, Tuple<int, String>> _ratings; //<username:String, <rating:int, review:String>>

        public Rating()
        {
            _ratings = new Dictionary<String, Tuple<int, String>>();
        }
    }
}
