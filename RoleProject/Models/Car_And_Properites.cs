using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RoleProject.Models
{
    public class Car_And_Properites
    {
        [Key]
        
        [Column(Order =1)]
        public int Car_Id { get; set; } // car id
        [Key]
        [Column(Order =2)]
        public int id { get; set; } // prop id 

        public Car Cars { get; set; }
        public Car_properties properties { get; set; }
      
       
    }
}