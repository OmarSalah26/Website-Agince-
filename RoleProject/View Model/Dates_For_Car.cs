using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RoleProject.Models;


    public class Dates_For_Car
    {
       
        public List<DateTime> Start_Recive { get; set; }
        public List<DateTime> End_Recive { get; set; }
        public List<Client> Clients { get; set; }
    public List<Double> Total_Price { get; set; }
    public Dates_For_Car()
    {
        Start_Recive = new List<DateTime>();
        End_Recive = new List<DateTime>();
        Clients = new List<Client>();
        Total_Price = new List<double>();
    }

}
