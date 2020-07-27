using RoleProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoleProject.Logic
{
    public class CarLogic
    {
        private ApplicationDbContext db;//= new ApplicationDbContext();

        public CarLogic()
        {
            db = new ApplicationDbContext();
        }

        public List<Car> GetAll()
        {
            return db.Cars.ToList();
        }
    }
}