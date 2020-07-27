using RoleProject.View_Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoleProject.Models
{
    public class ReciveDate
    {
        public int id { get; set; }
        [Display(Name = "Start Recive Date")]
 
        [Remote("StartDate", "Clients", ErrorMessage = "date is less than today")]
        //[startDateValidation]
        public DateTime Start_Recive_Date { get; set; }
        [Display(Name = "End Recive Date")]
        
                    [Remote("EndDate", "Clients", ErrorMessage = "date is less than today")]
        public DateTime End_Recive_Date { get; set; }
        public double Total_Cost { get; set; }
        public Client client { get; set; }
        public Car cars { get; set; }
    }
}