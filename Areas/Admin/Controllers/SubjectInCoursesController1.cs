using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sipl.DataBase;

namespace Sipl.Areas.Admin.Controllers
{
    [Authorize]
    public class SubjectInCoursesController1 : Controller
    {
        private SiplDatabaseEntities db = new SiplDatabaseEntities();

        // GET: Admin/SubjectInCourses1
        public ActionResult Index()
        {
            var subjectInCourse = db.SubjectInCourse.Include(s => s.Courses).Include(s => s.Subjects);
            return View(subjectInCourse.ToList());
        }

        // GET: Admin/SubjectInCourses1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectInCourse subjectInCourse = db.SubjectInCourse.Find(id);
            if (subjectInCourse == null)
            {
                return HttpNotFound();
            }
            return View(subjectInCourse);
        }

        // GET: Admin/SubjectInCourses1/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName");
            ViewBag.SubjectId = new SelectList(db.Subjects, "SubjectId", "SubjectName");
            return View();
        }

        // POST: Admin/SubjectInCourses1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SubjectId,CourseId")] SubjectInCourse subjectInCourse)
        {
            if (ModelState.IsValid)
            {
                db.SubjectInCourse.Add(subjectInCourse);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", subjectInCourse.CourseId);
            ViewBag.SubjectId = new SelectList(db.Subjects, "SubjectId", "SubjectName", subjectInCourse.SubjectId);
            return View(subjectInCourse);
        }

        // GET: Admin/SubjectInCourses1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectInCourse subjectInCourse = db.SubjectInCourse.Find(id);
            if (subjectInCourse == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", subjectInCourse.CourseId);
            ViewBag.SubjectId = new SelectList(db.Subjects, "SubjectId", "SubjectName", subjectInCourse.SubjectId);
            return View(subjectInCourse);
        }

        // POST: Admin/SubjectInCourses1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SubjectId,CourseId")] SubjectInCourse subjectInCourse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subjectInCourse).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", subjectInCourse.CourseId);
            ViewBag.SubjectId = new SelectList(db.Subjects, "SubjectId", "SubjectName", subjectInCourse.SubjectId);
            return View(subjectInCourse);
        }

        // GET: Admin/SubjectInCourses1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectInCourse subjectInCourse = db.SubjectInCourse.Find(id);
            if (subjectInCourse == null)
            {
                return HttpNotFound();
            }
            return View(subjectInCourse);
        }

        // POST: Admin/SubjectInCourses1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubjectInCourse subjectInCourse = db.SubjectInCourse.Find(id);
            db.SubjectInCourse.Remove(subjectInCourse);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        /// <param name="disposing"></param>
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
