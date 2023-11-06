using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ev_01.Models
{
    public class Food
    {
        public Food()
        {
            this.Receipts = new List<Receipt>();
        }

        public int FoodId { get; set; }
        public string FoodName { get; set; }

        public virtual ICollection<Receipt> Receipts { get; set; }
    }
    public class Order
    {
        public Order()
        {
            this.Receipts = new List<Receipt>();
        }
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string Picture { get; set; }

        [Required, Column(TypeName = "date"), Display(Name = "Date of Birth"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; }
        public int TableNumber { get; set; }
        public bool AmountPaid { get; set; }

        public virtual ICollection<Receipt> Receipts { get; set; }

    }
    public class Receipt
    {
        public int ReceiptId { get; set; }

        [ForeignKey("Food")]
        public int FoodId { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }

        //nav
        public virtual Food Food { get; set; }
        public virtual Order Order { get; set; }
    }

    public class ResturantDBcontext : DbContext
    {
        public DbSet<Food> Foods { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
    }
}