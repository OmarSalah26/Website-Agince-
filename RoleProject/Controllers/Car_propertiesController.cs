using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RoleProject.Models;

namespace RoleProject.Controllers
{
    [Authorize(Roles = "Agince")]
    public class Car_propertiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]

        public ActionResult List_Of_all(Car car)
        {
            try
            {
                Session["car_called"] = car;
                return View(db.Car_properties.ToList());
            }
            catch (Exception)
            {

                return RedirectToAction("error", "HomeFolder");
            }

        }

        public ActionResult Create()
        {
            return View();
        }



        [HttpPost]

        public ActionResult Create(Car_properties car_properties)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Car_properties.Add(car_properties);
                    db.SaveChanges();
                    return RedirectToAction("List_Of_All");
                }

                return View(car_properties);
            }
            catch (Exception)
            {

                return RedirectToAction("error", "HomeFolder");
            }

        }


        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("error", "HomeFolder");
                }
                Car_properties car_properties = db.Car_properties.Find(id);
                if (car_properties == null)
                {
                    return RedirectToAction("error", "HomeFolder");
                }
                return View(car_properties);
            }
            catch (Exception)
            {

                return RedirectToAction("error", "HomeFolder");
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Car_properties car_properties)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(car_properties).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("List_Of_All");
                }
                return View(car_properties);
            }
            catch (Exception)
            {

                return RedirectToAction("error", "HomeFolder");
            }

        }


        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("error", "HomeFolder");
                }
                Car_properties car_properties = db.Car_properties.Find(id);
                if (car_properties == null)
                {
                    return RedirectToAction("error", "HomeFolder");
                }
                return View(car_properties);
            }
            catch (Exception)
            {

                return RedirectToAction("error", "HomeFolder");
            }

        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Car_properties car_properties = db.Car_properties.Find(id);
                db.Car_properties.Remove(car_properties);
                db.SaveChanges();
                return RedirectToAction("List_Of_All");
            }
            catch (Exception)
            {

                return RedirectToAction("error", "HomeFolder");
            }

        }



    }
}
