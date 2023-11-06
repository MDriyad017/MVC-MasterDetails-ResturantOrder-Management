using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ev_01.Models.ViewModel
{
    public class OrderVM
    {
        public OrderVM()
        {
            this.FoodList = new List<int>();
        }
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string Picture { get; set; }
        public HttpPostedFileBase PictureFile { get; set; }

        [Required, Column(TypeName = "date"), Display(Name = "Date of Birth"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; }
        public int TableNumber { get; set; }
        public bool AmountPaid { get; set; }

        public List<int> FoodList { get; set; }
    }
}