using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RoleProject.View_Model;

using RoleProject.Models;
using Microsoft.AspNet.Identity;

namespace RoleProject.Controllers
{

    public class ClientsController : Controller
    {

      
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Clinets
       
            
        public ActionResult List_Of_All()
        {
            try
            {
                if (User.IsInRole("Client"))
                {
                    string id = User.Identity.GetUserId();


                    var client = db.Client.Where(e => e.Client_ID == id).ToList();

                    return View(client);


                }
                return View(db.Client.ToList());
            }
            catch (Exception)
            {
                return RedirectToAction("error", "HomeFolder");
            }
        }

        //Search
        [Authorize(Roles = "Admin")]
        public ActionResult Search(string searchItem)
        {
            try
            {

                return PartialView("_Search_Client_Partial");
            }
            catch (Exception)
            {
                return RedirectToAction("error", "HomeFolder");
            }
        }

        [Authorize(Roles = "Admin")]

        public ActionResult Go_Search(string searchItem)
        {

            try
            {
                var c = db.Client.Where(v => v.Client_ID == searchItem || v.city.Contains(searchItem) ||
                           v.street.Contains(searchItem) || v.Name.Contains(searchItem)).ToList();

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


        [Authorize(Roles = "Admin")]

        public ActionResult sorting()
        {
            try
            {
                return PartialView("_Sorting_clinet_Partial");
            }
            catch (Exception)
            {

                return RedirectToAction("error", "HomeFolder");
            }
        }

        public ActionResult Go_sorting(int? num)
        {

            try
            {

                if (num == null)
                {

                    return View("List_Of_All", db.Client.ToList());


                }
                else

                {

                    var clinet = new List<Client>();
                    switch (num)
                    {

                        case 1:
                            clinet = db.Client.OrderBy(e => e.age).ToList();
                            break;
                        case 2:
                            clinet = db.Client.OrderByDescending(e => e.age).ToList();
                            break;
                        case 3:
                            clinet = db.Client.OrderBy(e => e.city).ToList();
                            break;

                        case 4:
                            clinet = db.Client.OrderBy(e => e.street).ToList();
                            break;

                        case 5:
                            clinet = db.Client.OrderBy(e => e.Name).ToList();
                            break;

                        case 6:
                            clinet = db.Client.OrderBy(e => e.date_of_licience_expiry).ToList();
                            break;
                        case 7:
                            clinet = db.Client.OrderByDescending(e => e.date_of_licience_expiry).ToList();
                            break;
                    }



                    return View("List_Of_All", clinet);
                }
            }
            catch (Exception)
            {

                return RedirectToAction("error", "HomeFolder");
            }
        }
        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public ActionResult Details(String id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("error", "HomeFolder");
                }
                Recived_Cars_for_Client recived_Cars_For_Client = new Recived_Cars_for_Client();
                Client clinet = db.Client.FirstOrDefault(client_ => client_.Client_ID == id);

                //recived_Cars_For_Client.cars = db.Cars.Where(car => car.CLIENT.Client_ID == clinet.Client_ID).ToList();
                ViewBag.recived_Cars_For_Client = recived_Cars_For_Client.cars;
                if (clinet == null)
                {
                      return RedirectToAction("error", "HomeFolder");
                }
                return View(clinet);
            }
            catch (Exception)
            {

                return RedirectToAction("error", "HomeFolder");
            }
        }

     
        [AllowAnonymous]

        public ActionResult Edit(string id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("error", "HomeFolder");
                }
                Client Client = db.Client.Find(id);
                if (Client == null)
                {
                      return RedirectToAction("error", "HomeFolder");
                }
                return View(Client);

            }
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string Client_ID, Client client)
        {
            try
            {
                var newClient = db.Client.FirstOrDefault(client_ => client_.Client_ID == Client_ID);
                ApplicationUser newaginceAccount = db.Users.FirstOrDefault(_agince => _agince.Id == Client_ID);
              
                if (client.photo_path != null)
                {

                    /*Add photo to Data Base*/
                    string filename = Path.GetFileNameWithoutExtension(client.photo_path.FileName);
                    string Extintion = Path.GetExtension(client.photo_path.FileName);
                    filename = filename + DateTime.Now.ToString("yymmssfff") + Extintion;
                    newClient.photo_Client = filename;
                    filename = Path.Combine(Server.MapPath("~/images/"), filename);
                    client.photo_path.SaveAs(filename);
                    //----------------------------/
                }
                //newclientAccount.UserName = client.Name;
                //newclientAccount.PhoneNumber = client.phone_Number;
                //newclientAccount.street = client.street;
                //newclientAccount.city = client.city;
                newaginceAccount.UserName = client.Name;
                newaginceAccount.PhoneNumber = client.phone_Number;
                newaginceAccount.street = client.street;
                newaginceAccount.city = client.city;
                newaginceAccount.photoAdmin = newClient.photo_Client;

                db.SaveChanges();
                newClient.age = client.age;
                newClient.city = client.city;
                newClient.date_of_licience_expiry = client.date_of_licience_expiry;
                newClient.Name = client.Name;
                newClient.number_of_licience = client.number_of_licience;
                newClient.street = client.street;
                newClient.phone_Number = client.phone_Number;

                db.SaveChanges();

                return RedirectToAction("List_Of_All", "Cars");


            }
            catch
            {
                return View(client);
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
                Client clinet = db.Client.Find(id);
                if (clinet == null)
                {
                      return RedirectToAction("error", "HomeFolder");
                }
                return View(clinet);
            }
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult complete_data(String id, Client clinet)
        {

            try
            {
                var newclinet = db.Client.FirstOrDefault(clinet_ => clinet_.Client_ID == id);
                var newclinetUser = db.Users.FirstOrDefault(clinet_ => clinet_.Id == id);

                if (clinet.photo_path != null)
                {

                    /*Add photo to Data Base*/
                    string filename = Path.GetFileNameWithoutExtension(clinet.photo_path.FileName);
                    string Extintion = Path.GetExtension(clinet.photo_path.FileName);
                    filename = filename + DateTime.Now.ToString("yymmssfff") + Extintion;
                    newclinet.photo_Client = filename;
                    filename = Path.Combine(Server.MapPath("~/images/"), filename);
                    clinet.photo_path.SaveAs(filename);
                    //----------------------------/
                }
                newclinetUser.photoAdmin = newclinet.photo_Client;
                db.SaveChanges();
                newclinet.age = clinet.age;
                newclinet.date_of_licience_expiry = clinet.date_of_licience_expiry;

                db.SaveChanges();
                return RedirectToAction("login", "Account");
            }
            catch
            {
                return View("complete_data");
            }
        }



        public ActionResult Delete(String id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("error", "HomeFolder");
                }
                Client clinet = db.Client.Find(id);
                if (clinet == null)
                {
                      return RedirectToAction("error", "HomeFolder");
                }
                return View(clinet);
            }
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(String id)
        {
            try
            {
                Client clinet = db.Client.Find(id);
                db.Client.Remove(clinet);
                db.SaveChanges();
                return RedirectToAction("List_Of_All");
            }
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }
        }



        public ActionResult plus18(int Age)
        {
            try
            {

                if (Age >= 18)
                    return Json(true, JsonRequestBehavior.AllowGet);
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }
        }


        public ActionResult expireDate(DateTime date_of_licience_expiry)
        {
            try
            {

                if (date_of_licience_expiry > DateTime.Now.AddDays(30))

                    return Json(true, JsonRequestBehavior.AllowGet);
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }
        }

        public ActionResult StartDate(DateTime Start_Recive_Date)
        {
            try
            {

                if (Start_Recive_Date < DateTime.Now)

                    return Json(true, JsonRequestBehavior.AllowGet);
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }
        }
        public ActionResult EndDate(DateTime End_Recive_Date)
        {
            try
            {

                if (End_Recive_Date < DateTime.Now)

                    return Json(true, JsonRequestBehavior.AllowGet);
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }
        }
    }
}


