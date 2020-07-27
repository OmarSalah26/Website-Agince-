using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;

namespace RoleProject.Models
{
    public class Agince
    {
        public Agince()
        {
            Collection_Of_Car = new Collection<Car>();
        }
        [Key]
        [Required]

       
        public String Agince_ID { get; set; }
        [Required]

        public string name { get; set; }

        
        [Display(Name =  "Phone Number")]
       
        public string phone_number { get; set; }
   
        public string city { get; set; }
        //[Display(Name = "Total profits")]
        //public double Profits { get; set; }

        public string street { get; set; }
        [Display(Name ="photo of Agince")]
        public string photo_Agince { get; set; }
        [NotMapped]
        public HttpPostedFileBase photo_path { get; set; }
        // Collection of cars in Agince 
        public Collection<Car> Collection_Of_Car { get; set; }
        public virtual ApplicationUser userAccount { get; set; }
    }
}