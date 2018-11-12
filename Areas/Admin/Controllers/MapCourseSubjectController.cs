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
    public class MapCourseSubjectController : Controller
    {
        private SiplDatabaseEntities db = new SiplDatabaseEntities();
        /// <summary>
        /// list of Course in Subject
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMapCourseSubject()
        {
            //query for list
            var subjectInCourse = db.SubjectInCourse.Include(s => s.Courses).Include(s => s.Subjects);
            return View(subjectInCourse.ToList());
        }

        /// <summary>
        /// Get Method For Maping Course In Subject method
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateSubjectInCourse()
        {
            //for getting Course
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName");
            //for getting Subject
            ViewBag.SubjectId = new SelectList(db.Subjects, "SubjectId", "SubjectName");
            return View();
        }

       
        /// <summary>
        /// post method for mapping
        /// </summary>
        /// <param name="subjectInCourse"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSubjectInCourse([Bind(Include = "Id,SubjectId,CourseId")] SubjectInCourse subjectInCourse)
        {
            if (ModelState.IsValid)
            {
                db.SubjectInCourse.Add(subjectInCourse);
                db.SaveChanges();
                return RedirectToAction("GetMapCourseSubject");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", subjectInCourse.CourseId);
            ViewBag.SubjectId = new SelectList(db.Subjects, "SubjectId", "SubjectName", subjectInCourse.SubjectId);
            return View(subjectInCourse);
        }

   
        /// <summary>
        /// Update method for mapping  
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditSubjectInCourse(int? id)
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

        /// <summary>
        /// post method for Update
        /// </summary>
        /// <param name="subjectInCourse"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditSubjectInCourse([Bind(Include = "Id,SubjectId,CourseId")] SubjectInCourse subjectInCourse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subjectInCourse).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GetMapCourseSubject");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", subjectInCourse.CourseId);
            ViewBag.SubjectId = new SelectList(db.Subjects, "SubjectId", "SubjectName", subjectInCourse.SubjectId);

            return View(subjectInCourse);
        }

       /// <summary>
       /// Get method for delete 
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
        public ActionResult Delete(int id)
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

        /// <summary>
        /// delete post method 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubjectInCourse subjectInCourse = db.SubjectInCourse.Find(id);
            db.SubjectInCourse.Remove(subjectInCourse);
            db.SaveChanges();
            return RedirectToAction("GetMapCourseSubject");
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
