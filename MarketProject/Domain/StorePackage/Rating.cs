using System;
using System.Collections.Generic;

namespace MarketProject.Domain
{
    public class Rating
    {
        private Dictionary<String, Tuple<int, String>> _ratings; //<Username:String, <rating:int, review:String>>

        public Dictionary<String, Tuple<int, String>> Ratings => _ratings;

        public Rating()
        {
            _ratings = new Dictionary<String, Tuple<int, String>>();
        }
        public Rating(Dictionary<String, Tuple<int, String>> ratings)
        {
            _ratings = ratings;
        }

        public double GetRating()
        {
            double rating = 0;
            foreach (KeyValuePair<String, Tuple<int, String>> entry in _ratings)
            {
                rating += entry.Value.Item1;
            }
            rating /= _ratings.Count;
            return rating;
        }

        public String GetVisitorReview(String Username)
        {
            if (!_ratings.ContainsKey(Username))
                return "";
            Tuple<int, String> review = _ratings[Username];
            if (review.Item2 != "")
                return $"Visitor: {Username}\nGave a rating of: {review.Item1}\nWith a review:\n{review.Item2}\n";
            else
                return $"Visitor: {Username}\nGave a rating of: {review.Item1}\nWithout a review\n";
        }

        public bool AddRating(String Username, int rating, String review)
        {
            if (_ratings.ContainsKey(Username))
                return false;
            _ratings[Username] = new Tuple<int, String>(rating, review);
            return true;
        }

        public bool HasRating(String Username)
        {
            return _ratings.ContainsKey(Username);
        }
    }
}
