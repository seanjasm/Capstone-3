
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TaskManagerApp.Models;

namespace TaskManagerApp.Controllers
{
    public class TasksController : Controller
    {
        private TaskManagerDBEntities db = new TaskManagerDBEntities();

        // GET: Tasks
        public ActionResult Index()
        {
            Member member = (Member)Session["CurrentUser"];
            var tasks = db.Tasks.Where(m => m.MID == member.Id).Include(t => t.Member).ToList();
            ViewBag.StatusFilter = TaskFactory.GetStatusFilter();
            ViewBag.Result = "";
            return View(tasks);
        }

        [HttpPost]
        public ActionResult Index(string txtSearch, string date, string status)
        {
            Member member = (Member)Session["CurrentUser"];
            string statusMessage = "";

            var tasks = db.Tasks.Where(t => t.MID == member.Id &&
                t.description.Contains(txtSearch)).Include(t => t.Member);

            if(bool.TryParse(status, out bool Status))
            {
                tasks = tasks.Where(s => s.status == Status);
                statusMessage = status == "true" ? "Completed" : "Incompleted";
            }

            if (DateTime.TryParse(date, out DateTime Date))
            {
                tasks = tasks.Where(t => t.dueDate <= Date);
            }

            ViewBag.StatusFilter = TaskFactory.GetStatusFilter();
            ViewBag.Result = $"Last Search: {txtSearch}   Date: {date}   " +
                $"Status: {statusMessage}   Results found: {tasks.Count()}";
            return View(tasks.ToList());
        }

        // GET: Tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // GET: Tasks/Create
        public ActionResult Create()
        {
            //ViewBag.Members = MemberFactory.GetMemberSelectedListItem();
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,status,description,dueDate,MID")] Task task)
        {
            Member user = (Member)Session["CurrentUser"];
            task.MID = user.Id;

            if (ModelState.IsValid)
            {
                db.Tasks.Add(task);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.MID = new SelectList(db.Members, "Id", "firstName", task.MID);
            return View(task);
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            //ViewBag.MID = new SelectList(db.Members, "Id", "firstName", task.MID);
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,status,description,dueDate,MID")] Task task)
        {
            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MID = new SelectList(db.Members, "Id", "firstName", task.MID);
            return View(task);
        }

        // GET: Tasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Task task = db.Tasks.Find(id);
            db.Tasks.Remove(task);
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
