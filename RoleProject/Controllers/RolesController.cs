using Microsoft.AspNet.Identity.EntityFramework;
using RoleProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoleProject.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RolesController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public ActionResult List_Of_All()
        {
            return View(context.Roles.ToList());
        }


        public ActionResult Details(string id)
        {

            var role = context.Roles.Find(id);
            if (role == null) {
                return HttpNotFound();
            }
            else

            return View(role);
        }


        public ActionResult Create()
        {
            return View(new IdentityRole());
        }


        [HttpPost]
        public ActionResult Create(IdentityRole role)
        {



            if (ModelState.IsValid)
            {
                context.Roles.Add(role);
                context.SaveChanges();
                return RedirectToAction("List_Of_All");
            }
            else { return View(role); }

        }


        public ActionResult Edit(string id)
        {
            var role = context.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            else
                return View(role);
        }


        [HttpPost]
        public ActionResult Edit(string id, IdentityRole role)
        {

            if (ModelState.IsValid)
            {
                IdentityRole thenew = context.Roles.Find(id);
                thenew.Id = role.Id;
                thenew.Name = role.Name;
                context.SaveChanges();
                return RedirectToAction("List_Of_All");
            }
            else { return View(role); }



        }


        public ActionResult Delete(string id)
        {
            var role =context.Roles.FirstOrDefault(e => e.Id == id);
            if (role == null)
            {
                return HttpNotFound();
            }
            else
                return View(role);
        }


        [HttpPost]
        public ActionResult Delete(IdentityRole role)
        {

            if (ModelState.IsValid)
            {
                IdentityRole role_del = context.Roles.Find(role.Id);
                context.Roles.Remove(role_del);
                context.SaveChanges();
                return RedirectToAction("List_Of_All");
            }
            else { return View(role); }

        }
    }
}
