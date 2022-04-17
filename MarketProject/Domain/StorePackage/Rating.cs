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

        public float getRating()
        {
            float rating = 0;
            foreach (KeyValuePair<String, Tuple<int, String>> entry in _ratings)
            {
                rating += entry.Value.Item1;
            }
            rating /= _ratings.Count;
            return rating;
        }

        public String getUserReview(String username)
        {
            if (!_ratings.ContainsKey(username))
                return "";
            Tuple<int, String> review = _ratings[username];
            if (review.Item2 != "")
                return $"User: {username}\nGave a rating of: {review.Item1}\nWith a review:\n{review.Item2}\n";
            else
                return $"User: {username}\nGave a rating of: {review.Item1}\nWithout a review\n";
        }
    }
}
