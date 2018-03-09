using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BikeRentalASP.Controllers
{
    [Route("api")]
    public class CustomerController : Controller
    {
        // GET api/values
        [HttpGet]
        [Route("Customers")]
        public IActionResult Get([FromQuery] string filter)
        {
            DataContext db = new DataContext();
            if (filter != null)
            {
                return Ok(db.Customers.Where(c => c.LastName.Contains(filter)));

            }           
            return (Ok(db.Customers.ToList()));
        }

        // POST api/values
        [HttpPost]
        [Route("NewCustomer")]
        public IActionResult Post([FromBody]Customers customer)
        {
            DataContext db = new DataContext();
            db.Customers.Add(customer);
            db.SaveChanges();
            return Ok("Customer has been added.");
        }

        // PUT api/values/5
        [Route("UpdateCustomer/{id}")]
        [HttpPost]
        public IActionResult UpdateCustomer(int id, [FromBody]Customers value)
        {
            DataContext db = new DataContext();

            if(!db.Customers.Any(c => c.ID == id))
            {
                return BadRequest("No Customer found with given ID!");
            }

            var customer = db.Customers.SingleOrDefault(c => c.ID == id);

            customer.Birthday = value.Birthday;
            customer.FirstName = value.FirstName;
            customer.Gender = value.Gender;
            customer.HouseNumber = value.HouseNumber;
            customer.LastName = value.LastName;
            customer.Street = value.Street;
            customer.Town = value.Town;
            customer.Zipcode = value.Zipcode;           

            db.SaveChanges();

            return Ok("Customer has been updated!");
        }

        // DELETE api/values/5
        [Route("DeleteCustomer/{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            DataContext db = new DataContext();
            if (!db.Customers.Any(c => c.ID == id))
            {
                return BadRequest("No Customer found with given ID!");
            }

            if (db.Rentals.Any(c => c.Customer.ID == id))
            {
                return BadRequest("Cannot delete a Customer that has Rentals!");
            }

            db.Customers.Remove(db.Customers.Where(c => c.ID == id).FirstOrDefault());
            db.SaveChanges();


            return Ok("Customer deleted.");
        }

        [Route("Rentals/{id}")]
        [HttpGet]
        public IActionResult GetRentals(int id) //Bike und Customer null obwohl ID's in DB gesetzt sind
        {
            DataContext db = new DataContext();
            if (!db.Customers.Any(c => c.ID == id))
            {
                return BadRequest("No Customer found with given ID!");
            }

            return Ok(db.Rentals.Where(c => c.Customer.ID == id));
        }
    }
}
