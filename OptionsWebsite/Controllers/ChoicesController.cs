﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DiplomaDataModel;
using Microsoft.AspNet.Identity;
using OptionsWebsite.DataContext;

namespace OptionsWebsite.Controllers
{
    public class ChoicesController : Controller
    {
        private DiplomaContext db = new DiplomaContext();

        public ActionResult Complete()
        {
            return View();
        }

        public ActionResult Filter(string termFilter, string outFilter)
        {
            var terms = db.YearTerms.ToList();
            List<string> validTerms = new List<string>();

            YearTerm defaultTerm = db.YearTerms.Where(y => y.isDefault == true).FirstOrDefault();
            if (defaultTerm.Term == 10)
            {
                validTerms.Add("Winter " + defaultTerm.Year);
            }
            else if (defaultTerm.Term == 20)
            {
                validTerms.Add("Spring/Summer " + defaultTerm.Year);

            }
            else if (defaultTerm.Term == 30)
            {
                validTerms.Add("Fall " + defaultTerm.Year);
            }

            foreach (var t in terms)
            {
                if (t.Term == 10)
                {
                    validTerms.Add("Winter " + t.Year);
                }
                else if (t.Term == 20)
                {
                    validTerms.Add("Spring/Summer " + t.Year);

                }
                else if (t.Term == 30)
                {
                    validTerms.Add("Fall " + t.Year);
                }

            }

            List<string> outputs = new List<string>()
            {
                "Report",
                "Charts",
            };

            ViewBag.ValidOutput = new SelectList(outputs);
            ViewBag.ValidTerms = new SelectList(validTerms.Distinct());
            
            string[] tokens = termFilter.ToString().Split(' ');
            int year = Int32.Parse(tokens[1]);
            int term;
            if (tokens[0] == "Winter")
                term = 10;
            else if (tokens[0] == "Spring/Summer")
                term = 20;
            else
                term = 30;

            YearTerm yTerm = db.YearTerms.Where(y => y.Year == year && y.Term == term).FirstOrDefault();

            if (yTerm != null)
            {
                var choices = db.Choices.Where(t => t.YearTermId == yTerm.YearTermId).ToList();
                
                HashSet<Option> options = new HashSet<Option>();
                
                foreach(var choice in choices)
                {
                    options.Add(choice.FirstOption);
                    options.Add(choice.SecondOption);
                    options.Add(choice.ThirdOption);
                    options.Add(choice.FourthOption);
                }

                List<int> firstChoice = new List<int>();
                List<int> secondChoice = new List<int>();
                List<int> thirdChoice = new List<int>();
                List<int> fourthChoice = new List<int>();
                List<string> optionTitles = new List<string>();
                foreach (var option in options)
                {
                    optionTitles.Add(option.Title);
                    firstChoice.Add(db.Choices.Where(c => c.FirstOption.Title == option.Title
                    && c.YearTermId == yTerm.YearTermId).Count());
                    secondChoice.Add(db.Choices.Where(c => c.SecondOption.Title == option.Title
                    && c.YearTermId == yTerm.YearTermId).Count());
                    thirdChoice.Add(db.Choices.Where(c => c.ThirdOption.Title == option.Title
                    && c.YearTermId == yTerm.YearTermId).Count());
                    fourthChoice.Add(db.Choices.Where(c => c.FourthOption.Title == option.Title
                    && c.YearTermId == yTerm.YearTermId).Count());
                }
                
                ViewBag.FirstCount = firstChoice.ToArray();
                ViewBag.SecondCount = secondChoice.ToArray();
                ViewBag.ThirdCount = thirdChoice.ToArray();
                ViewBag.FourthCount = fourthChoice.ToArray();
                ViewBag.Opt = optionTitles.ToArray();

                if (Request.IsAjaxRequest())
                    return PartialView(outFilter, choices);
            }
            
            return PartialView("Report", db.Choices.ToList());
        }

        // GET: Choices
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var terms = db.YearTerms.ToList();
            List<string> validTerms = new List<string>();
           
            YearTerm defaultTerm = db.YearTerms.Where(y => y.isDefault == true).FirstOrDefault();
            if (defaultTerm.Term == 10)
            {
                validTerms.Add("Winter " + defaultTerm.Year);
            }
            else if (defaultTerm.Term == 20)
            {
                validTerms.Add("Spring/Summer " + defaultTerm.Year);

            }
            else if (defaultTerm.Term == 30)
            {
                validTerms.Add("Fall " + defaultTerm.Year);
            }

            foreach (var term in terms)
            {
                if (term.Term == 10)
                {
                    validTerms.Add("Winter " + term.Year);
                }
                else if (term.Term == 20)
                {
                    validTerms.Add("Spring/Summer " + term.Year);

                }
                else if (term.Term == 30)
                {
                    validTerms.Add("Fall " + term.Year);
                }

            }

            List<string> outputs = new List<string>()
            {
                "Report",
                "Charts",
            };

            ViewBag.ValidOutput = new SelectList(outputs);
            ViewBag.ValidTerms = new SelectList(validTerms.Distinct());

            var choices = db.Choices.Include(c => c.FirstOption).Include(c => c.FourthOption).Include(c => c.SecondOption).Include(c => c.ThirdOption).Include(c => c.YearTerm);
            return View(choices.ToList());
        }

        // GET: Choices/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Choice choice = db.Choices.Find(id);
            if (choice == null)
            {
                return HttpNotFound();
            }
            return View(choice);
        }

        // GET: Choices/Create
        public ActionResult Create()
        {
            var term = db.YearTerms.FirstOrDefault(y => y.isDefault == true);
            if (term.Term == 10)
            {
                ViewBag.YearTerm = "Winter " + term.Year;
            }
            else if (term.Term == 20)
            {
                ViewBag.YearTerm = "Spring/Summer " + term.Year;

            }
            else if (term.Term == 30)
            {
                ViewBag.YearTerm = "Fall " + term.Year;
            }
            var options = db.Options.Where(c => c.isActive == true);
            ViewBag.StudentId = User.Identity.GetUserName();
            ViewBag.FirstChoiceOptionId = new SelectList(options, "OptionId", "Title");
            ViewBag.SecondChoiceOptionId = new SelectList(options, "OptionId", "Title");
            ViewBag.ThirdChoiceOptionId = new SelectList(options, "OptionId", "Title");
            ViewBag.FourthChoiceOptionId = new SelectList(options, "OptionId", "Title");
            ViewBag.YearTermId = new SelectList(db.YearTerms, "YearTermId", "YearTermId");
            return View();
        }

        // POST: Choices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ChoiceId,YearTermId,StudentId,StudentFirstName,StudentLastName,FirstChoiceOptionId,SecondChoiceOptionId,ThirdChoiceOptionId,FourthChoiceOptionId,SelectionDate")] Choice choice)
        {
            var term = db.YearTerms.FirstOrDefault(y => y.isDefault == true);
            if (term.Term == 10)
            {
                ViewBag.YearTerm = "Winter " + term.Year;
            }
            else if (term.Term == 20)
            {
                ViewBag.YearTerm = "Spring/Summer " + term.Year;

            }
            else if (term.Term == 30)
            {
                ViewBag.YearTerm = "Fall " + term.Year;
            }


            var currentTerm = db.YearTerms.Where(c => c.isDefault == true).First();
            choice.YearTermId = currentTerm.YearTermId;
            var studentSubmit = from a in db.Choices
                                where a.StudentId == choice.StudentId && a.YearTermId == choice.YearTermId
                                select a;

            if (!studentSubmit.Any())
            {
                if (ModelState.IsValid)
                {
                    db.Choices.Add(choice);
                    choice.YearTerm = db.YearTerms.FirstOrDefault(c => c.isDefault == true);
                    db.SaveChanges();
                    return RedirectToAction("Complete");
                }
            }
            else
            {
                return View("Submitted");
            }

            var options = db.Options.Where(c => c.isActive == true);
            ViewBag.FirstChoiceOptionId = new SelectList(options, "OptionId", "Title", choice.FirstChoiceOptionId);
            ViewBag.SecondChoiceOptionId = new SelectList(options, "OptionId", "Title", choice.SecondChoiceOptionId);
            ViewBag.ThirdChoiceOptionId = new SelectList(options, "OptionId", "Title", choice.ThirdChoiceOptionId);
            ViewBag.FourthChoiceOptionId = new SelectList(options, "OptionId", "Title", choice.FourthChoiceOptionId);
            ViewBag.YearTermId = new SelectList(db.YearTerms, "YearTermId", "YearTermId", choice.YearTermId);
            return View(choice);

        }

        // GET: Choices/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Choice choice = db.Choices.Find(id);
            if (choice == null)
            {
                return HttpNotFound();
            }

            var options = db.Options.Where(c => c.isActive == true);
            ViewBag.FirstChoiceOptionId = new SelectList(options, "OptionId", "Title", choice.FirstChoiceOptionId);
            ViewBag.SecondChoiceOptionId = new SelectList(options, "OptionId", "Title", choice.SecondChoiceOptionId);
            ViewBag.ThirdChoiceOptionId = new SelectList(options, "OptionId", "Title", choice.ThirdChoiceOptionId);
            ViewBag.FourthChoiceOptionId = new SelectList(options, "OptionId", "Title", choice.FourthChoiceOptionId);
            ViewBag.YearTermId = new SelectList(db.YearTerms, "YearTermId", "YearTermId", choice.YearTermId);
            return View(choice);
        }

        // POST: Choices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ChoiceId,YearTermId,StudentId,StudentFirstName,StudentLastName,FirstChoiceOptionId,SecondChoiceOptionId,ThirdChoiceOptionId,FourthChoiceOptionId,SelectionDate")] Choice choice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(choice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var options = db.Options.Where(c => c.isActive == true);
            ViewBag.FirstChoiceOptionId = new SelectList(options, "OptionId", "Title", choice.FirstChoiceOptionId);
            ViewBag.SecondChoiceOptionId = new SelectList(options, "OptionId", "Title", choice.SecondChoiceOptionId);
            ViewBag.ThirdChoiceOptionId = new SelectList(options, "OptionId", "Title", choice.ThirdChoiceOptionId);
            ViewBag.FourthChoiceOptionId = new SelectList(options, "OptionId", "Title", choice.FourthChoiceOptionId);
            ViewBag.YearTermId = new SelectList(db.YearTerms, "YearTermId", "YearTermId", choice.YearTermId);
            return View(choice);
        }

        // GET: Choices/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Choice choice = db.Choices.Find(id);
            if (choice == null)
            {
                return HttpNotFound();
            }
            return View(choice);
        }

        // POST: Choices/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Choice choice = db.Choices.Find(id);
            db.Choices.Remove(choice);
            db.SaveChanges();
            return RedirectToAction("Index");
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
