using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BikeRentalASP
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        { }

        public DataContext()
        { }
        public DbSet<Customers> Customers { get; set; }

        public DbSet<Rentals> Rentals { get; set; }

        public DbSet<Bikes> Bikes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLOCALDB;Database=BikeDB;Trusted_Connection=True");
        }
        
    }
    public partial class Customers
    {
        public int ID { get; set; }
        
        public Genders Gender { get; set; }

        [MaxLength(50)]
        [Required]
        public string FirstName { get; set; }

        [MaxLength(75)]
        [Required]
        public string LastName { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Birthday { get; set; }

        [MaxLength(75)]
        [Required]
        public string Street { get; set; }

        [MaxLength(10)]
        public string HouseNumber { get; set; }

        [MaxLength(10)]
        [Required]
        public string Zipcode { get; set; }

        [MaxLength(75)]
        [Required]
        public string Town { get; set; }

    }

    public class Rentals
    {
        public int ID { get; set; }
        [Required]
        public Customers Customer { get; set; }

        [Required]
        public Bikes Bike { get; set; }
        [Required]
        public DateTime RentalBegin { get; set; }


        private DateTime? _RentalEnd;
        [Required]
        public DateTime? RentalEnd
        {
            get
            {
                return _RentalEnd;
            }
            set
            {
                if (value <= RentalBegin) //RentalEnd ungültig
                {
                    throw new Exception("RentalEnd is not valid!");
                }
                _RentalEnd = value;
            }
        }

        [Range(1, Double.PositiveInfinity)]
        public double? TotalCost { get; set; }

        public bool HasPaid { get; set; }




    }
    public class Bikes
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(25)]
        public string Brand { get; set; }

        [Column(TypeName = "date")]
        [Required]
        public DateTime PurchaseDate { get; set; }

        [MaxLength(1000)]
        public string Notes { get; set; }

        public DateTime? DateoflastService { get; set; }

        [Range(0,Double.PositiveInfinity)]
        [Required]
        public double PriceFirstHour {get; set;}

        [Range(1, Double.PositiveInfinity)]
        [Required]
        public double PriceForAdditionalHours { get; set; }

        public string Category { get; set; }

    }
    public partial class Customers
    {
        public enum Genders { Male, Female, Unknown };

    }
}
