using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TaskManagerApp.Models;

namespace TaskManagerApp.Controllers
{
    public class MembersController : Controller
    {
        private TaskManagerDBEntities db = new TaskManagerDBEntities();

        // GET: Members
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "firstName,lastName,Username,Password")] Member member)
        {
            if (ModelState.IsValid)
            {
                db.Members.Add(member);
                db.SaveChanges();
                member = MemberFactory.GetLoggedInMember(member.Username);
                Session["Currentuser"] = member;
                Session["UserName"] = member.firstName;
                return RedirectToAction("../Home/Index");
            }

            return View(member);
        }
                
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session["Currentuser"] = null;
            Session["UserName"] = null;
            return View("../Home/Index");
        }

        [HttpPost]
        public ActionResult Login(string Username, string Password)
        {
            Member user = db.Members.FirstOrDefault(m => m.Username == Username && m.Password == Password);
            if(user is null)
            {
                ViewBag.Message = "Login failed";
                return View();
            }
            Session["Currentuser"] = user;
            Session["UserName"] = user.firstName;
            return View("../Home/Index");
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
