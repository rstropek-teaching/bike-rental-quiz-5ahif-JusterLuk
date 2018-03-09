using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BikeRentalASP;

namespace BikeRentalASP.Controllers
{
    [Produces("application/json")]
    [Route("api/Bikes")]
    public class BikesController : Controller
    {

        // GET: api/Bikes
        [HttpGet]
        public IActionResult GetBikes([FromQuery] String s)
        {
            DataContext db = new DataContext();

            if (s == null)
            {
                return Ok(db.Bikes);
            }
            if (s.Equals("FirstHour"))
            {
                return Ok(db.Bikes.OrderBy(p => p.PriceFirstHour));
            }
            else if (s.Equals("AdditionalHour"))
            {
                return Ok(db.Bikes.OrderBy(p => p.PriceForAdditionalHours));
            }
            else if (s.Equals("PurchaseDate"))
            {
                return Ok(db.Bikes.OrderByDescending(p => p.PurchaseDate));
            }
            else
            {
                return BadRequest("Wrong sorting parameter was given!");
            }
        }

        [HttpPut]
        public IActionResult CreateBike([FromBody] Bikes bike)
        {
            DataContext db = new DataContext();

            if (bike == null)
            {
                return BadRequest("Invalid Bike!");
            }
            db.Bikes.Add(bike);
            db.SaveChanges();
            return Ok("Bike Created");
        }

        [HttpPut]
        [Route("{index}")]
        public IActionResult UpdateBike(int index, [FromBody]Bikes bike)
        {
            DataContext db = new DataContext();
            if (!db.Bikes.Any(a => a.ID == index))
            {
                return BadRequest("No Bike found");
            }
            var b = db.Bikes.Where(a => a.ID == index).FirstOrDefault();
            b.Brand = bike.Brand;
            b.Category = bike.Category;
            b.DateoflastService = bike.DateoflastService;
            b.Notes = bike.Notes;
            b.PriceForAdditionalHours = bike.PriceForAdditionalHours;
            b.PriceFirstHour = bike.PriceFirstHour;
            b.PurchaseDate = bike.PurchaseDate;
            db.SaveChanges();

            return Ok("Bike Updated");
        }

        [HttpDelete]
        [Route("{index}")]
        public IActionResult DeleteBike(int index)
        {
            DataContext db = new DataContext();
            if (!db.Bikes.Any(a => a.ID == index))
            {
                return BadRequest("No Bike found");
            }

            if (db.Rentals.Any(a => a.Bike.ID == index))
            {
                return BadRequest("Delete Not Possible: Bike is rented");
            }

            db.Bikes.Remove(db.Bikes.Where(a => a.ID == index).FirstOrDefault());
            db.SaveChanges();

            return Ok("Bike Deleted");
        }




    }
}