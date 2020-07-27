using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoleProject.Models
{
   
    public class Client
    {
        public Client()
        {
            Booked_Car = new Collection<ReciveDate>();
        }
      
        [Required]
        public string Name { get; set; }
        [Key]
        [Required]
        [Display(Name = "ID")]
        public String Client_ID { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone number")]

        public string phone_Number { get; set; }
        [Display(Name = "City")]
        public string city { get; set; }

        [Display(Name = "Street")]
        public string street { get; set; }

        [Display(Name = "Age")]
        [Remote("plus18", "Clients",ErrorMessage ="Age Cann't be less 18")]
        public Nullable<int> age { get; set; }

        [Display(Name = "Licience Number")]

        public Nullable<int> number_of_licience { get; set; }
        [Display(Name = "Date of Licience expiry")]
        
        [Remote("expireDate", "Clients", ErrorMessage = "Date is Expired After 30 Day ")]
        public Nullable<DateTime> date_of_licience_expiry { get; set; }

        public ICollection<ReciveDate> Booked_Car { get; set; }
        public string photo_Client { get; set; }
        [NotMapped]
        public HttpPostedFileBase photo_path { get; set; }

    }
}