using System;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using RoleProject.Models;
using RoleProject.View_Model;

namespace RoleProject.Controllers
{
  
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db;
        public AccountController()
        {
            db = new ApplicationDbContext();
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager , ApplicationDbContext db)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //Details
        public ActionResult Details(string id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var user = db.Users.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }
          
        }


        // to display all  user with role name
        public ActionResult UsersWithRoles()
        {
            try
            {
                var usersWithRoles = (from user in db.Users
                                      select new
                                      {
                                          UserId = user.Id,
                                          Username = user.UserName,
                                          Email = user.Email,
                                          city = user.city,
                                          street = user.street,
                                          phone_number = user.PhoneNumber,
                                          PhotoAdmin = user.photoAdmin,

                                          RoleNames = (from userRole in user.Roles
                                                       join role in db.Roles on userRole.RoleId
                                                       equals role.Id
                                                       select role.Name).ToList()
                                      }).ToList().Select(p => new Users_in_Role_ViewModel()

                                      {
                                          UserId = p.UserId,
                                          Username = p.Username,
                                          Email = p.Email,
                                          city = p.city,
                                          street = p.street,
                                          phone_number = p.phone_number,
                                          PhotoAdmin = p.PhotoAdmin,
                                          Role = string.Join(",", p.RoleNames)
                                      });

                return View(usersWithRoles);
            }
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }

         
        }










 

        [HttpGet]
        public ActionResult Edit(string id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ApplicationUser user = db.Users.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }
           
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(String id, ApplicationUser user)
        {
           

            var newUser = db.Users.FirstOrDefault(agince_ => agince_.Id == id);

            try
            {
                if (user.photo_path != null)
                {

                    /*Add photo to Data Base*/
                    string filename = Path.GetFileNameWithoutExtension(user.photo_path.FileName);
                    string Extintion = Path.GetExtension(user.photo_path.FileName);
                    filename = filename + DateTime.Now.ToString("yymmssfff") + Extintion;
                    newUser.photoAdmin = filename;
                    filename = Path.Combine(Server.MapPath("~/images/"), filename);
                    user.photo_path.SaveAs(filename);
                    //----------------------------/
                }
                newUser.UserName = user.UserName;
                newUser.PhoneNumber = user.PhoneNumber;
                newUser.street = user.street;
                newUser.city = user.city;
                db.SaveChanges();
                return RedirectToAction("UsersWithRoles");

            }
            catch (Exception)
            {

                return View(user);
            }
           
            



                
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            try
            {
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }
           
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true
                var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:

                        return RedirectToAction("index", "HomeFolder");
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                }
            }
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }
           
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }


        public ActionResult complete_data(string id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var User = db.Users.Find(id);
                if (User == null)
                {
                    return HttpNotFound();
                }
                return View(User);
            }
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }
          
        }

        // POST: Aginces/complete_data

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult complete_data(String id, ApplicationUser agince)
        {

            try
            {
                var newUser = db.Users.FirstOrDefault(agince_ => agince_.Id == id);

                if (agince.photo_path != null)
                {

                    /*Add photo to Data Base*/
                    string filename = Path.GetFileNameWithoutExtension(agince.photo_path.FileName);
                    string Extintion = Path.GetExtension(agince.photo_path.FileName);
                    filename = filename + DateTime.Now.ToString("yymmssfff") + Extintion;
                    newUser.photoAdmin = filename;
                    filename = Path.Combine(Server.MapPath("~/images/"), filename);
                    agince.photo_path.SaveAs(filename);
                    //----------------------------/
                }
                db.SaveChanges();
                return RedirectToAction("login", "Account");
            }
            catch
            {
                return View("complete_data");
            }
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            try
            {
                if (!User.IsInRole("Admin"))
                    ViewBag.Roles = new SelectList(context.Roles.Where(e => e.Name != "Admin").ToList(), "Name", "Name");

                else
                    ViewBag.Roles = new SelectList(context.Roles.Where(e => e.Name == "Admin").ToList(), "Name", "Name");

                return View();
            }
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }

           
        }
        ApplicationDbContext context = new ApplicationDbContext();

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {


                    if (model.Roles == "Client" && ModelState.IsValid)
                    {

                        Client clinet = new Client();

                        if (db.Client.FirstOrDefault(w => w.Client_ID == model.Id) != null)
                        {
                            db.Client.Remove(db.Client.FirstOrDefault(w => w.Client_ID == model.Id));
                            db.SaveChanges();
                            clinet.Client_ID = (model.Id); //  اقصد به رقم البطاقه
                            clinet.Name = model.UserName;
                            //clinet.password = model.Password;
                            clinet.phone_Number = model.PhoneNumber;
                            clinet.city = model.city;
                            clinet.street = model.Street;
                            context.Client.Add(clinet);
                            context.SaveChanges();
                        }
                        else
                        {
                            clinet.Client_ID = (model.Id); //  اقصد به رقم البطاقه
                            clinet.Name = model.UserName;
                            // clinet.password = model.Password;
                            clinet.phone_Number = model.PhoneNumber;
                            clinet.city = model.city;
                            clinet.street = model.Street;
                            context.Client.Add(clinet);
                            context.SaveChanges();
                        }


                    }


                    else if (model.Roles == "Agince" && ModelState.IsValid)
                    {
                        Agince aganis_Car = new Agince();
                        if (db.Agince.FirstOrDefault(w => w.Agince_ID == model.Id) != null)
                        {
                            db.Agince.Remove(db.Agince.FirstOrDefault(w => w.Agince_ID == model.Id));
                            db.SaveChanges();
                            aganis_Car.Agince_ID = (model.Id);
                            aganis_Car.name = model.UserName;
                            //aganis_Car.password = model.Password;
                            aganis_Car.phone_number = model.PhoneNumber;
                            aganis_Car.city = model.city;
                            aganis_Car.street = model.Street;
                            context.Agince.Add(aganis_Car);
                            context.SaveChanges();
                        }
                        else
                        {
                            aganis_Car.Agince_ID = (model.Id);
                            aganis_Car.name = model.UserName;
                            // aganis_Car.password = model.Password;
                            aganis_Car.phone_number = model.PhoneNumber;
                            aganis_Car.city = model.city;
                            aganis_Car.street = model.Street;
                            context.Agince.Add(aganis_Car);
                            context.SaveChanges();
                        }

                    }


                    var user = new ApplicationUser
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        Id = model.Id,
                        PhoneNumber = model.PhoneNumber,
                        city = model.city,
                        street = model.Street
                    };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                        // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");



                        this.UserManager.AddToRole(user.Id, model.Roles);
                        if (model.Roles == "Agince")
                        {
                            return RedirectToAction("complete_data", "Aginces", new { id = user.Id });
                        }
                        else if (model.Roles == "Client")
                        {
                            return RedirectToAction("complete_data", "Clients", new { id = user.Id });
                        }
                        else if (model.Roles == "Admin") { return RedirectToAction("complete_data", "Account", new { id = user.Id }); }


                    }

                    if (!User.IsInRole("Admin"))
                        ViewBag.Roles = new SelectList(context.Roles.Where(e => e.Name != "Admin").ToList(), "Name", "Name");

                    else
                        ViewBag.Roles = new SelectList(context.Roles.Where(e => e.Name == "Admin").ToList(), "Name", "Name"); ;
                    AddErrors(result);
                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }
          
        }



              [AllowAnonymous]
        




        // Serach
 
        public ActionResult Search(string searchItem)
        {
            try
            {

                return PartialView("_Search_Users_Partial");
            }
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }

        }
        [AllowAnonymous]

        public ActionResult Go_Search(string searchItem)
        {

            try
            {


                var usersWithRoles = (from user in db.Users
                                      select new
                                      {
                                          UserId = user.Id,
                                          Username = user.UserName,
                                          Email = user.Email,
                                          city = user.city,
                                          street = user.street,
                                          phone_number = user.PhoneNumber,
                                          PhotoAdmin = user.photoAdmin,
                                          RoleNames = (from userRole in user.Roles
                                                       join role in db.Roles on userRole.RoleId
                                                       equals role.Id
                                                       select role.Name).ToList()
                                      }).ToList().Select(p => new Users_in_Role_ViewModel()

                                      {
                                          UserId = p.UserId,
                                          Username = p.Username,
                                          Email = p.Email,
                                          city = p.city,
                                          street = p.street,
                                          phone_number = p.phone_number,
                                          PhotoAdmin = p.PhotoAdmin,
                                          Role = string.Join(",", p.RoleNames)
                                      }).Where(e => e.city.Contains(searchItem) || e.street.Contains(searchItem) || e.Role.Contains(searchItem) ||
                                    e.UserId.Contains(searchItem) || e.Username.Contains(searchItem) || e.Email.Contains(searchItem)).ToList();


                if (usersWithRoles == null)
                {
                    return View("UsersWithRoles");
                }
                else




                    return View("UsersWithRoles", usersWithRoles);

            }
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }
        }


        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        { 
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {                   ViewBag.Roles = new SelectList(context.Roles.ToList(), "Name", "Name");

            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model/*, string Roles*/)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

     

            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }


        public ActionResult Delete(string id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ApplicationUser user = db.Users.Find(id);

                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }

        
        }

        // POST: Aginces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                if (db.Agince.Find(id) != null)
                {

                    db.Agince.Remove(db.Agince.Find(id));
                    db.Users.Remove(db.Users.Find(id));
                    db.SaveChanges();

                }

                else if (db.Client.Find(id) != null)
                {

                    db.Client.Remove(db.Client.Find(id));
                    db.Users.Remove(db.Users.Find(id));
                    db.SaveChanges();

                }
                else if (db.Users.Find(id) != null)
                {

                    db.Users.Remove(db.Users.Find(id));
                    db.SaveChanges();

                }



                return RedirectToAction("UsersWithRoles");
            }
            catch (Exception)
            {

                  return RedirectToAction("error", "HomeFolder");
            }

          
        }

        public ActionResult sorting()
        {
            try
            {
                return PartialView("_Sorting_Users_Partial");
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

                return RedirectToAction("UsersWithRoles");

            }
            else

            {
                var users = new List<ApplicationUser>();
                if (num == 1)
                {
                    var usersWithRoles = (from user in db.Users
                                          select new
                                          {
                                              UserId = user.Id,
                                              Username = user.UserName,
                                              Email = user.Email,
                                              city = user.city,
                                              street = user.street,
                                              phone_number = user.PhoneNumber,
                                              PhotoAdmin=user.photoAdmin,
                                              RoleNames = (from userRole in user.Roles
                                                           join role in db.Roles on userRole.RoleId
                                                           equals role.Id
                                                           select role.Name).ToList()
                                          }).ToList().Select(p => new Users_in_Role_ViewModel()

                                          {
                                              UserId = p.UserId,
                                              Username = p.Username,
                                              Email = p.Email,
                                              city = p.city,
                                              street = p.street,
                                              phone_number = p.phone_number,
                                              PhotoAdmin = p.PhotoAdmin,
                                              Role = string.Join(",", p.RoleNames)
                                          }).OrderBy(e => e.Username);
                    return View("UsersWithRoles", usersWithRoles);
                }

                else if (num == 2)
                {
                    var usersWithRoles = (from user in db.Users
                                          select new
                                          {
                                              UserId = user.Id,
                                              Username = user.UserName,
                                              Email = user.Email,
                                              city = user.city,
                                              street = user.street,
                                              phone_number = user.PhoneNumber,
                                              PhotoAdmin = user.photoAdmin,
                                              RoleNames = (from userRole in user.Roles
                                                           join role in db.Roles on userRole.RoleId
                                                           equals role.Id
                                                           select role.Name).ToList()
                                          }).ToList().Select(p => new Users_in_Role_ViewModel()

                                          {
                                              UserId = p.UserId,
                                              Username = p.Username,
                                              Email = p.Email,
                                              city = p.city,
                                              street = p.street,
                                              phone_number = p.phone_number,
                                              PhotoAdmin = p.PhotoAdmin,
                                              Role = string.Join(",", p.RoleNames)
                                          }).OrderBy(e => e.city);
                    return View("UsersWithRoles", usersWithRoles);
                }

                else if (num == 3)
                {
                    var usersWithRoles = (from user in db.Users
                                          select new
                                          {
                                              UserId = user.Id,
                                              Username = user.UserName,
                                              Email = user.Email,
                                              city = user.city,
                                              street = user.street,
                                              phone_number = user.PhoneNumber,
                                              PhotoAdmin = user.photoAdmin,
                                              RoleNames = (from userRole in user.Roles
                                                           join role in db.Roles on userRole.RoleId
                                                           equals role.Id
                                                           select role.Name).ToList()
                                          }).ToList().Select(p => new Users_in_Role_ViewModel()

                                          {
                                              UserId = p.UserId,
                                              Username = p.Username,
                                              Email = p.Email,
                                              city = p.city,
                                              street = p.street,
                                              phone_number = p.phone_number,
                                              PhotoAdmin = p.PhotoAdmin,
                                              Role = string.Join(",", p.RoleNames)
                                          }).OrderBy(e => e.street);
                    return View("UsersWithRoles", usersWithRoles);
                }

                else if (num == 4)
                {
                    var usersWithRoles = (from user in db.Users
                                          select new
                                          {
                                              UserId = user.Id,
                                              Username = user.UserName,
                                              Email = user.Email,
                                              city = user.city,
                                              street = user.street,
                                              phone_number = user.PhoneNumber,
                                              PhotoAdmin = user.photoAdmin,
                                              RoleNames = (from userRole in user.Roles
                                                           join role in db.Roles on userRole.RoleId
                                                           equals role.Id
                                                           select role.Name).ToList()
                                          }).ToList().Select(p => new Users_in_Role_ViewModel()

                                          {
                                              UserId = p.UserId,
                                              Username = p.Username,
                                              Email = p.Email,
                                              city = p.city,
                                              street = p.street,
                                              phone_number = p.phone_number,
                                              PhotoAdmin = p.PhotoAdmin,
                                              Role = string.Join(",", p.RoleNames)
                                          }).OrderBy(e => e.Role);
                    return View("UsersWithRoles", usersWithRoles);
                }
                else if (num == 5)
                {
                    var usersWithRoles = (from user in db.Users
                                          select new
                                          {
                                              UserId = user.Id,
                                              Username = user.UserName,
                                              Email = user.Email,
                                              city = user.city,
                                              street = user.street,
                                              phone_number = user.PhoneNumber,
                                              PhotoAdmin = user.photoAdmin,
                                              RoleNames = (from userRole in user.Roles
                                                           join role in db.Roles on userRole.RoleId
                                                           equals role.Id
                                                           select role.Name).ToList()
                                          }).ToList().Select(p => new Users_in_Role_ViewModel()

                                          {
                                              UserId = p.UserId,
                                              Username = p.Username,
                                              Email = p.Email,
                                              city = p.city,
                                              street = p.street,
                                              phone_number = p.phone_number,
                                              PhotoAdmin = p.PhotoAdmin,
                                              Role = string.Join(",", p.RoleNames)
                                          }).OrderBy(e => e.UserId);
                    return View("UsersWithRoles", usersWithRoles);
                }
                else { return View("UsersWithRoles"); }




            }
        }
        catch (Exception)
        {

              return RedirectToAction("error", "HomeFolder");
        }


        }



   
    #region Helpers
    // Used for XSRF protection when adding external logins
    private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}