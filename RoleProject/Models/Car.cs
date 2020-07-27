using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RoleProject.Models
{
    public class Car
    {
        public Car()
        {
            reciveDates = new Collection<ReciveDate>();
        }
       
        [Key]
        [Required]
        [Display(Name = "Car Number")]

        public int Car_Id { get; set; }

        [Required]
        [Display(Name = "Type of Car")]
        public string Type_Of_Car { get; set; }

        [Required]
        [Display(Name = "Car Brand")]

        public string Car_Brand { get; set; }
        [Required]
        [Display(Name = "Car Model")]

        public string Car_Model { get; set; }
        [Required]
        [Display(Name = "Chassis number")]

        public string Chassis_No { get; set; }
        

        [Required]
        [Display(Name = "Price per day")]

        public double price_in_day { get; set; }
 
       
        
       
        public virtual ICollection<Car_And_Properites> Additional_properties { get; set; }
        public string photo_Car { get; set; }
        [NotMapped]
        public HttpPostedFileBase photo_path { get; set; }


        public ICollection<ReciveDate> reciveDates { get; set; }
        public string Agince_ID { get; set; }
        public Agince Agince_Of_Car { get; set; }

    }
}