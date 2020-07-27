using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using RoleProject.Models;
using RoleProject.View_Model;

namespace RoleProject.Controllers
{
    public class AgincesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        [AllowAnonymous]

        public ActionResult List_Of_All()
        {
            try
            {

                var agince = db.Agince.ToList();

                return View(agince);
            }
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }






        }



        [AllowAnonymous]

        public ActionResult sorting()
        {
            try
            {
                return PartialView("_Sorting_Agince_Partial");

            }
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }
        }
        [AllowAnonymous]
        public ActionResult Go_sorting(int? num)
        {
            try
            {

                if (num == null)
                {

                    return View("List_Of_All", db.Agince.ToList());


                }
                else

                {
                    var Agincetemp = TempData["Aginces"] as List<Agince>;
                    var agince = new List<Agince>();
                    switch (num)
                    {

                        case 1:
                            agince = Agincetemp.OrderBy(e => e.name).ToList();
                            break;
                        case 2:
                            agince = Agincetemp.OrderBy(e => e.city).ToList();
                            break;



                        case 3:
                            agince = Agincetemp.OrderBy(e => e.street).ToList();
                            break;



                    }



                    return View("List_Of_All", agince);
                }

            }
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }

        }



        [AllowAnonymous]
        public ActionResult Search_byName(string searchItem)
        {
            try
            {
                return PartialView("_Search_Aginces_ByName_Partial");
            }
         
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }
}
        [AllowAnonymous]

        public ActionResult Go_Search_byName(string searchItem)
        {
            try
            {
                var Agincetemp = TempData["Aginces"] as List<Agince>;

                var c = Agincetemp.Where(n => n.name.Contains(searchItem) || n.city.Contains(searchItem)
                || n.street.Contains(searchItem)).ToList();
                if (c == null)
                {
                    return View("SearchError");// go to error page

                }
                else
                    return View("List_Of_All", c);
            }
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }

          
        }

        public ActionResult DetailsToUser(string id) {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("error", "HomeFolder");
                }
                Agince agince = db.Agince.Find(id);
                if (agince == null)
                {
                      return RedirectToAction("error", "HomeFolder");
                }
                ViewBag.Cars = (from w in db.Cars
                                where w.Agince_Of_Car.Agince_ID == id
                                select w).ToList();
                ViewBag.Aginces = db.Agince.Take(3).ToList();
                return View("AginceToUser", agince);
            }
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }
        }
        [Authorize(Roles = "Admin,Agince")]
      
        public ActionResult Details(string id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("error", "HomeFolder");
                }
                Agince agince = db.Agince.Find(id);
                if (agince == null)
                {
                      return RedirectToAction("error", "HomeFolder");
                }
                return View(agince);
            }
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }
        }

        
        [AllowAnonymous]

        public ActionResult complete_data(string id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("error", "HomeFolder");
                }
                Agince agince = db.Agince.Find(id);
                if (agince == null)
                {
                      return RedirectToAction("error", "HomeFolder");
                }
                return View(agince);
            }
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult complete_data(String id, Agince agince)
        {

            try
            {
                var newAgince = db.Agince.FirstOrDefault(agince_ => agince_.Agince_ID == id);
               

                if (agince.photo_path != null)
                {

                    /*Add photo to Data Base*/
                    string filename = Path.GetFileNameWithoutExtension(agince.photo_path.FileName);
                    string Extintion = Path.GetExtension(agince.photo_path.FileName);
                    filename = filename + DateTime.Now.ToString("yymmssfff") + Extintion;
                    newAgince.photo_Agince = filename;
                    filename = Path.Combine(Server.MapPath("~/images/"), filename);
                    agince.photo_path.SaveAs(filename);
                    //----------------------------/
                }
                var newAginceUser = db.Users.FirstOrDefault(agince_ => agince_.Id == id);

                newAginceUser.photoAdmin = newAgince.photo_Agince;
                db.SaveChanges();
                db.SaveChanges();
                return RedirectToAction("login", "Account");
            }
            catch
            {
                return View("complete_data");
            }
        }

        [Authorize(Roles = "Admin,Agince")]
  
        public ActionResult Edit(string id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("error", "HomeFolder");
                }
                Agince agince = db.Agince.Find(id);
                if (agince == null)
                {
                      return RedirectToAction("error", "HomeFolder");
                }
                return View(agince);
            }
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }
        }

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Agince")]
        public ActionResult Edit(String id, Agince agince)
        {

            try
            {
                var newAgince = db.Agince.FirstOrDefault(agince_ => agince_.Agince_ID == id);
                ApplicationUser newaginceAccount = db.Users.FirstOrDefault(_agince => _agince.Id == id);

                if (agince.photo_path != null)
                {
                   
                    /*Add photo to Data Base*/
                    string filename = Path.GetFileNameWithoutExtension(agince.photo_path.FileName);
                    string Extintion = Path.GetExtension(agince.photo_path.FileName);
                    filename = filename + DateTime.Now.ToString("yymmssfff") + Extintion;
                    newAgince.photo_Agince = filename;
                    filename = Path.Combine(Server.MapPath("~/images/"), filename);
                    agince.photo_path.SaveAs(filename);
                    //----------------------------/
                }
           
                newaginceAccount.UserName = agince.name;
                newaginceAccount.PhoneNumber = agince.phone_number;
                newaginceAccount.street = agince.street;
                newaginceAccount.city = agince.city;
                newaginceAccount.photoAdmin = newAgince.photo_Agince;
                db.SaveChanges();
                newAgince.name = agince.name;
                newAgince.phone_number = agince.phone_number;
                newAgince.street = agince.street;
                newAgince.city = agince.city;

                db.SaveChanges();
                return RedirectToAction("List_Of_All");
            }
            catch
            {
                return View("Edit");
            }
        }
        [Authorize(Roles = "Admin,Agince")]
       
        public ActionResult Delete(string id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("error", "HomeFolder");
                }
                Agince agince = db.Agince.Find(id);
                if (agince == null)
                {
                      return RedirectToAction("error", "HomeFolder");
                }
                return View(agince);
            }
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Agince")]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                Agince agince = db.Agince.Find(id);
                db.Agince.Remove(agince);
                db.SaveChanges();

                return RedirectToAction("List_Of_All");
            }
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }

        }





        public ActionResult report()
        {
            try
            {
                var aginceid = User.Identity.GetUserId();
                List<Report_Agince> report_agince = new List<Report_Agince>();
                var agince_cars = db.Cars.Where(c => c.Agince_Of_Car.Agince_ID == aginceid).ToList();

                Dates_For_Car _date;
                foreach (var car in agince_cars)
                {

                    _date = new Dates_For_Car();

                    _date.Start_Recive = (from dates in db.ReciveDates
                                          where (dates.cars.Car_Id == car.Car_Id)
                                          select
                                          (
                                             dates.Start_Recive_Date
                                              )).ToList();
                    _date.End_Recive = (from dates in db.ReciveDates
                                        where (dates.cars.Car_Id == car.Car_Id)
                                        select
                                        (
                                           dates.End_Recive_Date
                                            )).ToList();
                    _date.Clients = (from dates in db.ReciveDates
                                     where (dates.cars.Car_Id == car.Car_Id)
                                     select
                                     (
                                        dates.client
                                         )).ToList();
                    _date.Total_Price =
                        (from dates in db.ReciveDates
                         where (dates.cars.Car_Id == car.Car_Id)
                         select
                         (
                            dates.Total_Cost
                             )).ToList();


                    Report_Agince report = new Report_Agince
                    {
                        Clients = _date.Clients,
                        Total_Price = _date.Total_Price,
                        End_Recive = _date.End_Recive,
                        Start_Recive = _date.Start_Recive,
                        Car = car,
                    };
                    report_agince.Add(report);

                }
                Agince agince = db.Agince.FirstOrDefault(ag => ag.Agince_ID == aginceid);
                ViewBag.Agince = agince.photo_Agince;
                ViewBag.AginceID = agince.Agince_ID;

                return View(report_agince);

            }
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }


        }



    }
}
