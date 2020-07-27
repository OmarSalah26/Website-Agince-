using RoleProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoleProject.Controllers
{
    public class HomeFolderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            ViewBag.Aginces = db.Agince.ToList();
            return View("index",db.Cars.ToList());
        }

        public ActionResult AboutUS()
        {
            

            return View();
        }

        public ActionResult Contact()
        {
           

            return View();
        }

        public ActionResult error()
        {


            return View();
        }
    }
}