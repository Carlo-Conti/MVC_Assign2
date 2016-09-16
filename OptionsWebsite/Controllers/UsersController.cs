using Microsoft.AspNet.Identity.EntityFramework;
using OptionsWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Validation;
using Microsoft.AspNet.Identity;

namespace OptionsWebsite.Controllers
{
    public class UsersController : Controller
    {

        private ApplicationDbContext context = new ApplicationDbContext();

        // GET: /Users/Index
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var users = context.Users.ToList();
            return View(users);
        }

        // GET: Users/Edit
        [Authorize(Roles = "Admin")]
        public ActionResult Edit()
        {
            var users = context.Users.ToList();
            List<string> usernames = new List<string>();
            foreach (var user in users)
            {
                usernames.Add(user.UserName);
            }

            var roles = context.Roles.ToList();
            List<string> valid = new List<string>();
            foreach (var role in roles)
            {
                valid.Add(role.Name);
            }

            ViewBag.UserRoles = TempData["UserRoles"];
            ViewBag.Message = TempData["Message"];
            ViewBag.GetMsg = TempData["GetMsg"];
            ViewBag.UserChoice = new SelectList(usernames);
            ViewBag.RoleChoice = new SelectList(valid);
            return View();
        }

        // POST: Users/Edit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRole(string userName, string roleName)
        {
            var user = context.Users.Where(u => u.UserName == userName).FirstOrDefault();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            userManager.AddToRole(userManager.FindByEmail(user.Email).Id, roleName);
            TempData["Message"] = "- " + roleName + " role added to user " + userName;

            return RedirectToAction("Edit");
        }

        // POST: Users/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string userName)
        {
            var user = context.Users.Where(u => u.UserName == userName).FirstOrDefault();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            TempData["GetMsg"] = "Roles for user " + userName + ":";
            TempData["UserRoles"] = userManager.GetRoles(user.Id);

            return RedirectToAction("Edit");
        }

        // POST: Users/Edit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRole(string userName, string roleName)
        {
            if (userName == "A00111111" && roleName == "Admin")
                TempData["Message"] = "- " + roleName + " role cannot be deleted from user " + userName;
            else {
                var user = context.Users.Where(u => u.UserName == userName).FirstOrDefault();
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                userManager.RemoveFromRole(userManager.FindByEmail(user.Email).Id, roleName);
                TempData["Message"] = "- " + roleName + " role removed from user " + userName;
            }
            return RedirectToAction("Edit");
        }

        // GET: Users/Details
        [Authorize(Roles = "Admin")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = context.Users.Find(id);
            //var user = context.Users.Where(r => r.Id.Equals(id, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Lockout
        [Authorize(Roles = "Admin")]
        public ActionResult Lockout(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = context.Users.Find(id);
            //var user = context.Users.Where(r => r.Id.Equals(id, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Lockout
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Lockout([Bind(Include = "Id, UserName, Email, PasswordHash, SecurityStamp, LockoutEnabled ")] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                if (user.LockoutEnabled == true)
                    user.LockoutEndDateUtc = DateTime.UtcNow.AddYears(420);
                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

    }
}