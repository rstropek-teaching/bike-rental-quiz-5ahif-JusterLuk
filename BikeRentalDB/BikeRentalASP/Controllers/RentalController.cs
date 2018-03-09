using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BikeRentalASP.Controllers
{
    [Route("api/Rentals")]
    public class RentalController : Controller
    {

        [HttpGet]
        [Route("StartRent")]
        public IActionResult StartRental([FromQuery] int CustomerID, int BikeID)
        {
            DataContext db = new DataContext();

            if (!db.Customers.Any(a => a.ID == CustomerID) || !db.Bikes.Any(a => a.ID == BikeID))
            {
                return BadRequest("Wrong Bike or Customer given!");
            }
            if (db.Rentals.Any(a => a.Customer.ID == CustomerID))
            {
                return BadRequest("This Customer has no rentals!");
            }
            if (db.Rentals.Any(a => a.Bike.ID == BikeID))
            {
                return BadRequest("This Bike is not rented!");
            }

            Rentals rent = new Rentals()
            {
                Customer = db.Customers.First(a => a.ID == CustomerID),
                Bike = db.Bikes.First(a => a.ID == BikeID),
                RentalBegin = DateTime.Now,
                //RentalEnd = new DateTime(2000, 1, 1), //null doesn't work
                HasPaid = false,
                TotalCost = null
            };

            db.Rentals.Add(rent);
            db.SaveChanges();

            return Ok("Rent has successfully started!");
        }

        [HttpGet]
        [Route("StopRent")]
        public IActionResult StopRental([FromQuery]int CustomerID, int BikeID)
        {
            DataContext db = new DataContext();

            var rent = db.Rentals.First(a => a.Customer.ID == CustomerID & a.Bike.ID == BikeID);
            rent.RentalEnd = DateTime.Now;

            double cost = CalculateCost(rent, db.Bikes.First(a => a.ID == BikeID));
            rent.TotalCost = cost;
            db.SaveChanges();

            return Ok(rent);
        }

        [NonAction]
        public double CalculateCost(Rentals rent, Bikes bike)
        {
            DataContext db = new DataContext();

            TimeSpan? nullable = rent.RentalEnd - rent.RentalBegin;
            TimeSpan t = nullable.Value; //Workaround from nullable Timestamp
            double min = t.TotalMinutes;
            double cost = 0;

            if (min <= 15) //free if under 16
            {
                return 0;
            }

            cost = cost + bike.PriceFirstHour; //First Hour is different
            min = min - 60;

            while (min > 0)
            {
                cost = cost + bike.PriceForAdditionalHours;
                min = min - 60;
            }

            return cost;
        }


        [HttpGet]
        [Route("PaidRentals")]
        public IActionResult MarkPaid([FromQuery]int rentalID)
        {
            DataContext db = new DataContext();
            Rentals rental;

            try
            {
                rental = db.Rentals.Where(p => p.ID == rentalID).ToArray().Single();

                if (rental.TotalCost == 0)
                {
                    return BadRequest("There are no costs to pay for!");
                }
                else if (rental.RentalEnd == null)
                {
                    return BadRequest("This rental is still active!");
                }
                else if (rental.HasPaid)
                {
                    return BadRequest("Rental already paid!");
                }

                rental.HasPaid = true;

                db.Rentals.Update(rental);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest("Rental could not be paid!");
            }

            return Ok("Paid");
        }


        [HttpGet]
        [Route("UnpaidRents")]
        public IActionResult GetUnpaid()
        {
            DataContext db = new DataContext();
            return Ok(db.Rentals.Where(a => a.HasPaid == false && a.RentalEnd > a.RentalBegin && a.RentalEnd != null && a.TotalCost > 0).SelectMany(a => new Object[] { a.Customer.ID, a.Customer.FirstName, a.Customer.LastName, a.ID, a.RentalBegin, a.RentalEnd, a.TotalCost }));
        }


    }
}
