using Microsoft.VisualStudio.TestTools.UnitTesting;
using MarketProject.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.Tests
{
    [TestClass()]
    public class RatingTests
    {
        [TestMethod()]
        public void AddRating_UserHasntRatedYet_ReturnsTrue()
        {
            Rating _rating = new Rating();
            String username = "SpongeBob SquarePants";
            int rating = 2;
            String review = "";

            bool result = _rating.AddRating(username, rating, review);

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void AddRating_UserRatedAlready_ReturnsFalse()
        {
            Rating _rating = new Rating();
            String username = "SpongeBob SquarePants";
            int rating = 2;
            String review = "Not bad...";
            _rating.AddRating(username, rating + 1, "");

            bool result = _rating.AddRating(username, rating, review);

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void GetRating_AllUsersRated5_Returns5()
        {
            Rating _rating = new Rating();
            String username = "SpongeBob SquarePants";
            int rating = 5;
            String review = "Not bad...";
            _rating.AddRating(username, rating, review);
            username = "Patrick Star";
            _rating.AddRating(username, rating, review);
            username = "Mr. Krab";
            _rating.AddRating(username, rating, review);

            float result = _rating.GetRating();

            Assert.AreEqual(result, 5.0);
        }
    }
}