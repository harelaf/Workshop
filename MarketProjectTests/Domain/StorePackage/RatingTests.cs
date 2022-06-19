using Microsoft.VisualStudio.TestTools.UnitTesting;
using MarketProject.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using MarketWeb.Server.Domain;

namespace MarketProject.Domain.Tests
{
    [TestClass()]
    public class RatingTests
    {
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Req II 3.3 ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        // ===================================== AddRating =====================================
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


        // ===================================== GetRating =====================================
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