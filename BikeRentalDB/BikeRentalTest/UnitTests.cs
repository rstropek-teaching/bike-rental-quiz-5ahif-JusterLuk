using BikeRentalASP;
using BikeRentalASP.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BikeRentalTest
{
    [TestClass]
    public class UnitTests
    {
        RentalController controller = new RentalController();

        private Customers customer = new Customers()
        {
            Gender = Customers.Genders.Male,
            FirstName = "Max",
            LastName = "Mustermann",
            Birthday = new DateTime(1990, 5, 25),
            Street = "Musterstraﬂe",
            HouseNumber = "15",
            Zipcode = "4312",
            Town = "Ried"
        };

        private Bikes bike = new Bikes()
        {
            Brand = "KTM",
            PurchaseDate = new DateTime(2018, 3, 1),
            Notes = "Schoenes Bike",
            DateoflastService = new DateTime(2018, 3, 5),
            PriceFirstHour = 3,
            PriceForAdditionalHours = 5,
            Category = "Mountain Bike"
        };
        [TestMethod]
        public void MoreThanOneHour()
        {

            Rentals rental = new Rentals()
            {
                Customer = customer,
                Bike = bike,
                RentalBegin = new DateTime(2018, 2, 14, 8, 15, 0),
                RentalEnd = new DateTime(2018, 2, 14, 10, 30, 0),
            };

            Assert.AreEqual(13, controller.CalculateCost(rental, bike));

        }

        [TestMethod]
        public void OneHour()
        {

            Rentals rental = new Rentals()
            {
                Customer = customer,
                Bike = bike,
                RentalBegin = new DateTime(2018, 2, 14, 8, 15, 0),
                RentalEnd = new DateTime(2018, 2, 14, 8, 45, 0),
            };

            Assert.AreEqual(3, controller.CalculateCost(rental, bike));

        }

        [TestMethod]
        public void Below15()
        {

            Rentals rental = new Rentals()
            {
                Customer = customer,
                Bike = bike,
                RentalBegin = new DateTime(2018, 2, 14, 8, 15, 0),
                RentalEnd = new DateTime(2018, 2, 14, 8, 25, 0),
            };

            Assert.AreEqual(0, controller.CalculateCost(rental, bike));

        }

    }

}

