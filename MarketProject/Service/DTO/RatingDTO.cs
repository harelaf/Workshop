using MarketProject.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Service.DTO
{
    public class RatingDTO
    {
        private Dictionary<String, Tuple<int, String>> _ratings;

        public Dictionary<String, Tuple<int, String>> Ratings => _ratings;

        public RatingDTO(Rating rating)
        {
            _ratings = rating.Ratings;
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

        public String GetUserReview(String username)
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
