
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWebProject.DTO
{
    public class RatingDTO
    {
        private Dictionary<String, Tuple<int, String>> _ratings;

        public Dictionary<String, Tuple<int, String>> Ratings => _ratings;

        public RatingDTO()
        {
            _ratings = new Dictionary<String, Tuple<int, String>>();
        }

        public float GetRating()
        {
            float rating = 0;
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
    }
}
