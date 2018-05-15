using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineWebApp;
using System.Web.Security;


namespace OnlineWebApp.Controllers
{
    public class LoginController : Controller
    {
        private QuizDBContext db = new QuizDBContext();

        // GET: Login/Register
        public ActionResult Register(string ReturnUrl = "~/Home/Index")
        {
            if (User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectFromLoginPage(User.Identity.Name, false);
            }
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        // POST: Login/Register
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Username,Password,FirstName,LastName")] User userIn, string ReturnUrl = "~/Home/Index")
        {
            if (User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectFromLoginPage(User.Identity.Name, false);
            }
            if (ModelState.IsValid)
            {
                //This has been already done by the 
                User user = (from u in db.Users
                             where u.Username.Equals(userIn.Username)
                             select u).FirstOrDefault<User>();
                if(user == null)
                {
                    db.Users.Add(userIn);
                    db.SaveChanges();
                    FormsAuthentication.RedirectFromLoginPage(userIn.Username, false);
                } else
                {
                    ModelState.AddModelError("", "User already exists");
                }
            }
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        // GET: Login/Login
        public ActionResult Login(string ReturnUrl = "~/Home/Index")
        {
            if (User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectFromLoginPage(User.Identity.Name, false);
            }
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        // Copied from slides
        // Credits to Jaya
        //
        // POST: Login/Login
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Username, Password")] User userIn, string ReturnUrl = "~/Home/Index")
        {
            if (User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectFromLoginPage(User.Identity.Name, false);
            }
            ModelState.Remove("FirstName");
            ModelState.Remove("LastName");
            if (ModelState.IsValid)
            {
                //check if valid user password pair. If it is, log them into the site and return them back
                //username is unique
                User user = (from u in db.Users
                             where u.Username.Equals(userIn.Username)
                             select u).FirstOrDefault<User>();
                if (user != null) // found a match
                    if (user.Password.Equals(userIn.Password))
                        //log into site:
                        FormsAuthentication.RedirectFromLoginPage(userIn.Username, false);
            }
            //still here: either user not found, or password didn’t match
            ViewBag.ReturnUrl = ReturnUrl;
            ModelState.AddModelError("", "Invalid user name or password");
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
