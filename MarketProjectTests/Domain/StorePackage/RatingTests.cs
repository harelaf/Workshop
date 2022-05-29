using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MarketProject.Domain.Tests
{
    [TestClass()]
    public class RatingTests
    {
        [TestMethod()]
        public void AddRating_VisitorHasntRatedYet_ReturnsTrue()
        {
            Rating _rating = new Rating();
            String Username = "SpongeBob SquarePants";
            int rating = 2;
            String review = "";

            bool result = _rating.AddRating(Username, rating, review);

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void AddRating_VisitorRatedAlready_ReturnsFalse()
        {
            Rating _rating = new Rating();
            String Username = "SpongeBob SquarePants";
            int rating = 2;
            String review = "Not bad...";
            _rating.AddRating(Username, rating + 1, "");

            bool result = _rating.AddRating(Username, rating, review);

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void GetRating_AllVisitorsRated5_Returns5()
        {
            Rating _rating = new Rating();
            String Username = "SpongeBob SquarePants";
            int rating = 5;
            String review = "Not bad...";
            _rating.AddRating(Username, rating, review);
            Username = "Patrick Star";
            _rating.AddRating(Username, rating, review);
            Username = "Mr. Krab";
            _rating.AddRating(Username, rating, review);

            double result = _rating.GetRating();

            Assert.AreEqual(result, 5.0);
        }
    }
}