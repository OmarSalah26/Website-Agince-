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


    public class CarsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        [AllowAnonymous]


        public ActionResult List_Of_All()
        {
            try
            {
                var cars = new List<Car>();

                if (User.IsInRole("Agince"))
                {
                    string id = User.Identity.GetUserId();

                    cars = (from w in db.Cars
                            where w.Agince_Of_Car.Agince_ID == id
                            select w).ToList();

                }
                else
                {


                    cars = db.Cars.ToList();

                }

                return View(cars);
            }
            catch (Exception)
            {

                return RedirectToAction("error", "HomeFolder");
            }


        }

        public ActionResult List_of_Car_inowner_Agince(string id)
        {
            try
            {
                var cars = (from w in db.Cars
                            where w.Agince_Of_Car.Agince_ID == id
                            select w).ToList();


                return View("List_Of_All", cars);
            }
            catch (Exception)
            {

                return RedirectToAction("error", "HomeFolder");
            }


        }



        [AllowAnonymous]

        public ActionResult filter()
        {
            try
            {
                return PartialView("_filter_Car", db.Car_properties.ToList());
            }
            catch (Exception)
            {

                return RedirectToAction("error", "HomeFolder");
            }

        }
        [AllowAnonymous]

        public ActionResult go_filter(String prop_name)
        {
            try
            {
                var carstemp = TempData["cars"] as List<Car>;
                int prop_id = db.Car_properties.FirstOrDefault(e => e.proprity_Name == prop_name).id;


                var carsIds = (from e in db.Car_And_Properites
                               where e.id == prop_id
                               select e.Car_Id).ToList();

                List<Car> cars = new List<Car>();
                foreach (var item in carsIds)
                {
                    Car res = db.Cars.FirstOrDefault(e => e.Car_Id == item);
                    cars.Add(res);

                }

                return View("List_Of_all", cars);
            }
            catch (Exception)
            {

                return RedirectToAction("error", "HomeFolder");
            }




        }
        // search
        [AllowAnonymous]

        public ActionResult Search()
        {

            try
            {
                return PartialView("_Search_Car_Partial");
            }
            catch (Exception)
            {

                return RedirectToAction("error", "HomeFolder");
            }


        }

        [AllowAnonymous]


        public ActionResult Go_Search(string type, string brand, string model_ofCar, int? price)
        {
            try
            {
                List<Car> cars = db.Cars.Where(x => x.Type_Of_Car.Contains(type) && x.Car_Brand.Contains(brand)
           && x.Car_Model.Contains(model_ofCar) && x.price_in_day <= price).ToList();

                return View("List_Of_All", cars);
            }
            catch (Exception)
            {

                return RedirectToAction("error", "HomeFolder");
            }




        }

        //// search
        [AllowAnonymous]

        public ActionResult Search_By_Chassia()
        {
            try
            {
                return PartialView("_Search_Car_By_Chassia_Partial");
            }
            catch (Exception)
            {

                return RedirectToAction("error", "HomeFolder");
            }



        }
        [AllowAnonymous]


        public ActionResult Go_Search_By_Chassia(string Chassis, string type, string model, string brand)/*, int? num)*/
        {
            try
            {
                List<Car> cars = db.Cars.Where(x => x.Chassis_No == Chassis && x.Car_Model.Contains(model) && x.Car_Brand.Contains(brand) | x.Type_Of_Car.Contains(type)).ToList();

                if (cars != null)
                    return View("List_Of_All", cars);
                else
                    return View("List_Of_All", db.Cars.ToList());

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
                return PartialView("_Sorting_Partial");
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

                    return View("List_Of_All", db.Cars.ToList());


                }

                else

                {

                    var cars = new List<Car>();

                    var carstemp = TempData["cars"] as List<Car>;


                    switch (num)
                    {

                        case 1:
                            cars = carstemp.OrderBy(e => e.price_in_day).ToList();
                            break;
                        case 2:
                            cars = carstemp.OrderByDescending(e => e.price_in_day).ToList();
                            break;
                        case 3:
                            cars = carstemp.OrderBy(e => e.Car_Brand).ToList();
                            break;

                        case 4:
                            cars = carstemp.OrderBy(e => e.Car_Model).ToList();
                            break;
                        case 5:
                            cars = carstemp.OrderByDescending(e => e.Car_Model).ToList();
                            break;


                    }



                    return View("List_Of_All", cars);
                }
            }
            catch (Exception)
            {

                return RedirectToAction("error", "HomeFolder");
            }


        }


        [AllowAnonymous]


        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("error", "HomeFolder");
                }
                Car car = db.Cars.FirstOrDefault(Car_ => Car_.Car_Id == id);
                cars_and_properties cars_And_Properties = new cars_and_properties();

                cars_And_Properties.Property_Name =
                    (from car_ in db.Car_And_Properites
                     where (car_.Cars.Car_Id == car.Car_Id)
                     select (
                     car_.properties.proprity_Name
                            )).ToList();

                Dates_For_Car Date = new Dates_For_Car();
                Date.Start_Recive = (from Dates in db.ReciveDates
                                     where (Dates.cars.Car_Id == car.Car_Id)
                                     select
                                     (
                                        Dates.Start_Recive_Date
                                         )).ToList();
                Date.End_Recive = (from Dates in db.ReciveDates
                                   where (Dates.cars.Car_Id == car.Car_Id)
                                   select
                                   (
                                      Dates.End_Recive_Date
                                       )).ToList();
                Date.Clients = (from Dates in db.ReciveDates
                                where (Dates.cars.Car_Id == car.Car_Id)
                                select
                                (
                                   Dates.client
                                    )).ToList();
                Agince agince = (from ca in db.Cars
                                 from ag in db.Agince
                                 where (ag.Agince_ID == ca.Agince_Of_Car.Agince_ID)
                                 select (ag)).FirstOrDefault();

                ViewBag.Agince = agince;
                ViewBag.list_of_Recived_Date = Date;
                ViewBag.list_of_properties = cars_And_Properties;
                ViewBag.Allcars = db.Cars.Take(8).ToList();


                if (car == null)
                {
                    return RedirectToAction("error", "HomeFolder");
                }
                return View(car);
            }
            catch (Exception)
            {

                return RedirectToAction("error", "HomeFolder");
            }

        }



        [Authorize(Roles = "Agince")]

        public ActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {

                return RedirectToAction("error", "HomeFolder");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Agince")]

        public ActionResult Create(Car car)
        {
            try
            {
                string Agince_ID = User.Identity.GetUserId();
                Agince agince = db.Agince.FirstOrDefault(agince_ => agince_.Agince_ID == Agince_ID);
                if (car.photo_path != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(car.photo_path.FileName);
                    string Extintion = Path.GetExtension(car.photo_path.FileName);
                    filename = filename + DateTime.Now.ToString("yymmssfff") + Extintion;
                    car.photo_Car = filename;
                    filename = Path.Combine(Server.MapPath("~/images/"), filename);
                    car.photo_path.SaveAs(filename);
                }
                car.Agince_Of_Car = agince;
                agince.Collection_Of_Car.Add(car);
                db.Cars.Add(car);
                if (agince.Agince_ID != null)
                {
                    car.Agince_Of_Car = agince;
                }

                db.SaveChanges();
                return RedirectToAction("List_Of_All", "Car_properties", car);
            }
            catch
            {
                return View(car);
            }
        }
        [Authorize(Roles = "Agince,Admin")]

        public ActionResult Add_properity(int? id)
        {
            try
            {
                Car car;
                car = Session["car_called"] as Car;


                var properity = db.Car_properties.FirstOrDefault(prop => prop.id == id);
                Car_And_Properites car_And_Properites = new Car_And_Properites();
                car_And_Properites.Car_Id = car.Car_Id;
                car_And_Properites.properties = properity;
                db.Car_And_Properites.Add(car_And_Properites);

                db.SaveChanges();

                return RedirectToAction("List_Of_all", "Car_properties", car);
            }
            catch (Exception)
            {

                return RedirectToAction("error", "HomeFolder");
            }

        }

        public ActionResult Delete_properity_From_Car(int? id)
        {
            try
            {
                Car car;
                car = Session["car_called"] as Car;


                var properity = db.Car_And_Properites.FirstOrDefault(prop => prop.Car_Id == car.Car_Id && prop.id == id);

                db.Car_And_Properites.Remove(properity);

                db.SaveChanges();

                return RedirectToAction("List_Of_all", "Car_properties", car);
            }
            catch (Exception)
            {

                return RedirectToAction("error", "HomeFolder");
            }

        }

        [Authorize(Roles = "Agince")]



        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("error", "HomeFolder");
                }
                Car car = db.Cars.Find(id);
                if (car == null)
                {
                    return RedirectToAction("error", "HomeFolder");
                }
                return View(car);
            }
            catch (Exception)
            {

                return RedirectToAction("error", "HomeFolder");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Agince")]
        public ActionResult Edit(int? id, Car car)
        {

            try
            {
                //return car to edit
                var Car_Edititing = db.Cars.FirstOrDefault(car_ => car_.Car_Id == id);
                // update data in car
                Car_Edititing.Car_Brand = car.Car_Brand;
                Car_Edititing.Car_Model = car.Car_Model;
                Car_Edititing.Chassis_No = car.Chassis_No;
                Car_Edititing.price_in_day = car.price_in_day;



                if (car.photo_path != null)
                {
                    //to update photo-------------------------------------------------
                    string filename = Path.GetFileNameWithoutExtension(car.photo_path.FileName);
                    string Extintion = Path.GetExtension(car.photo_path.FileName);
                    filename = filename + DateTime.Now.ToString("yymmssfff") + Extintion;
                    Car_Edititing.photo_Car = filename;
                    filename = Path.Combine(Server.MapPath("~/images/"), filename);
                    car.photo_path.SaveAs(filename);
                    //-----------------------------------------------------------------
                }

                // save update
                db.SaveChanges();
                return RedirectToAction("List_Of_All", "Car_properties", Car_Edititing);
            }
            catch
            {

                return View(car);
            }
        }
        [Authorize(Roles = "Agince")]


        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("error", "HomeFolder");
                }
                Car car = db.Cars.Find(id);
                if (car == null)
                {
                    return RedirectToAction("error", "HomeFolder");
                }
                return View(car);
            }
            catch (Exception)
            {

                return RedirectToAction("error", "HomeFolder");
            }

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Agince")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Car car = db.Cars.Find(id);
                db.Cars.Remove(car);
                db.SaveChanges();
                return RedirectToAction("List_Of_All");
            }
            catch (Exception)
            {

                return RedirectToAction("error", "HomeFolder");
            }

        }
        [Authorize(Roles = "Client")]

        public ActionResult Recive(int? id)
        {
            try
            {
                var Cars_Recived = db.Cars.FirstOrDefault(car => car.Car_Id == id);
                if (Cars_Recived != null)
                {


                    return View(Cars_Recived);// display View of recive



                }
                else
                {
                    return View("Car_Not_Found"); // dispaly Cars is Not Fount ;

                }
            }
            catch (Exception)
            {

                return RedirectToAction("error", "HomeFolder");
            }

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Client,Agince")]
        public ActionResult Recive(int? Car_Id, DateTime Start_Recive_Date, DateTime End_Recive_Date)
        {

            try
            {


                var Cars_Recived = db.Cars.FirstOrDefault(car => car.Car_Id == Car_Id);
                if ((Start_Recive_Date > End_Recive_Date) || Start_Recive_Date < DateTime.Now) { return View(Cars_Recived); }
                ReciveDate reciveDate = new ReciveDate();
                reciveDate.Start_Recive_Date = Start_Recive_Date;
                reciveDate.End_Recive_Date = End_Recive_Date;
                reciveDate.cars = Cars_Recived;
                Dates_For_Car Date = new Dates_For_Car();
                Date.Start_Recive = (from Dates in db.ReciveDates
                                     where (Dates.cars.Car_Id == Cars_Recived.Car_Id)
                                     select
                                     (
                                        Dates.Start_Recive_Date
                                         )).ToList();
                Date.End_Recive = (from Dates in db.ReciveDates
                                   where (Dates.cars.Car_Id == Cars_Recived.Car_Id)
                                   select
                                   (
                                      Dates.End_Recive_Date
                                       )).ToList();

                ViewBag.Agince = db.Agince.FirstOrDefault(agince => agince.Agince_ID == Cars_Recived.Agince_ID);

                foreach (var start in Date.Start_Recive)
                {
                    foreach (var end in Date.End_Recive)
                    {
                        if ((Start_Recive_Date >= start) && (End_Recive_Date <= end))
                        {
                            return View("Recive_Alredy_Done", Cars_Recived);//display Cars is Elredy recirved
                        }
                        else
                        {
                            continue;
                        }
                    }

                }


                string client_id = User.Identity.GetUserId();
                Client CLIENT = db.Client.FirstOrDefault(client_ => client_.Client_ID == client_id);
                reciveDate.client = CLIENT;
                Calc_price(reciveDate);
                Cars_Recived.reciveDates.Add(reciveDate);
                CLIENT.Booked_Car.Add(reciveDate);
                db.SaveChanges();

                ViewBag.startDate = Start_Recive_Date;
                ViewBag.endDate = End_Recive_Date;
                ViewBag.client = CLIENT.Name;


                return View("Displa_Reciving", Cars_Recived);// display confirm for client recived done

            }
            catch (Exception)
            {

                return RedirectToAction("error", "HomeFolder");
            }


        }
        [Authorize(Roles = "Client,Agince")]
        public ActionResult DetailsCancel(int? id)
        {
            ViewBag.CarID = id;
            return View();
        }
        [Authorize(Roles = "Client,Agince")]


        public ActionResult CancelReciveFirst(int? id, DateTime Start_Recive_Date)
        {
            try
            {
                var Cars_Recived = db.Cars.FirstOrDefault(car => car.Car_Id == id);
                if (Cars_Recived != null)
                {
                    string client_id = User.Identity.GetUserId();
                    Client CLIENT = db.Client.FirstOrDefault(client_ => client_.Client_ID == client_id);
                    ReciveDate reciveDate = db.ReciveDates.Where(ReciveDate => ReciveDate.Start_Recive_Date == Start_Recive_Date && ReciveDate.client.Client_ID == CLIENT.Client_ID).FirstOrDefault();
                    ViewBag.Agince = db.Agince.FirstOrDefault(agince => agince.Agince_ID == Cars_Recived.Agince_ID);
                    if (reciveDate != null)
                    {
                        ViewBag.ReciveDate = reciveDate;
                        return View("CancelRecive", Cars_Recived);// display View of recive
                    }
                    else
                    {
                        return View("Recive_Not_Done_Yet", Cars_Recived);//display Cars is not recirved yet
                    }
                }
                else
                {
                    return View("Car_Not_Found"); // dispaly Cars is Not Fount 
                }
            }
            catch (Exception)
            {

                return RedirectToAction("error", "HomeFolder");
            }

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Client,Agince")]

        public ActionResult CancelRecive(ReciveDate reciveDate)
        {
            try
            {
                reciveDate = TempData["ReciveDate"] as ReciveDate;
                ReciveDate Rea_reciveDate = (from reciveDate_ in db.ReciveDates
                                             where (reciveDate_.id == reciveDate.id)
                                             select (
                                             reciveDate_
                                             )).FirstOrDefault();
                Car Cars_Recived = (from reciveDate_ in db.ReciveDates
                                    where (reciveDate_.id == reciveDate.id)
                                    select (
                                    reciveDate_.cars
                                    )).FirstOrDefault();

                if (Rea_reciveDate != null)
                {

                    if (Rea_reciveDate.Start_Recive_Date <= DateTime.Now)
                    {
                        Rea_reciveDate.End_Recive_Date = DateTime.Now;
                        Calc_price(Rea_reciveDate);

                        db.SaveChanges();
                        return View("bill", Cars_Recived);
                    }
                    else
                    {
                        Rea_reciveDate.Start_Recive_Date = DateTime.Now;
                        Rea_reciveDate.End_Recive_Date = DateTime.Now;
                        db.SaveChanges();
                        return RedirectToAction("List_of_all");//display Cancel is Done with no cost 

                    }
                }
                else
                {
                    return View("Recive_Not_Done_Yet", Cars_Recived);//display Cars is not recirved yet
                }


            }
            catch
            {
                return View("Car_Not_Found"); // dispaly Cars is Not Fount 
            }
        }


        [Authorize(Roles = "Client,Agince")]


        public void Calc_price(ReciveDate reciveDate)
        {

            int days_is_recived = reciveDate.End_Recive_Date.Subtract(reciveDate.Start_Recive_Date).Days;
            reciveDate.Total_Cost = days_is_recived * reciveDate.cars.price_in_day;




        }



    }
}