using Sipl.Areas.Admin.Models;
using Sipl.DataBase;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Sipl.Areas.Admin.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        SiplDatabaseEntities objEntities = new SiplDatabaseEntities();
        /// <summary>
        ///  List of all Courses Added by Admin
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCourse()
        {
            List<CourseViewModel> objCourseViewModel = new List<CourseViewModel>();
            var data = (from p in objEntities.Courses select p).ToList();
            foreach (var item in data)
            {
                CourseViewModel course = new CourseViewModel
                {
                    CourseId = item.CourseId,
                    CourseName = item.CourseName
                  
                };
                objCourseViewModel.Add(course);
            };
            return View(objCourseViewModel);
        }
        /// <summary>
        /// GET: Create Method For Course
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateCourse(string id)
        {
           return View();
        }
        /// <summary>
        /// POST :Create Method For Course
        /// </summary>
        /// <param name="objCourseViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCourse(CourseViewModel objCourseViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Courses objCourses = new Courses
                    {
                        CourseName = objCourseViewModel.CourseName,
                     
                    };
                    var test = objEntities.Courses.Add(objCourses);
                    objEntities.SaveChanges();
                    
                };
                return RedirectToAction("GetCourse", "Course", objCourseViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// get method for update 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditCourse(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Courses courses = objEntities.Courses.Find(id);
            var data = from d in objEntities.Courses
                       where d.CourseId == id
                       select d;
            //var TEMPlIST = objEntities.Subjects.ToList();
            CourseViewModel courseView = new CourseViewModel
            {
                CourseId = Convert.ToInt32(courses.CourseId),
                CourseName = courses.CourseName
            
            };
            if (courses == null)
            {
                return HttpNotFound();
            }
          
            return View(courseView);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="objCourses"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditCourse(int id, Courses objCourses)

        {
            {
                if (ModelState.IsValid)

                {
                    Courses objCourse = new Courses
                    {
                        CourseId = id,
                        CourseName = objCourses.CourseName
                       



                    };
                    objEntities.Entry(objCourse).State = EntityState.Modified;
                    objEntities.SaveChanges();
                    return RedirectToAction("GetCourse");
                }
                return View(objCourses);
            }

        }
        /// <summary>
        /// Delete method for only courses
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteCourse(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Courses courses = objEntities.Courses.Find(id);
            var data = from d in objEntities.Courses
                       where d.CourseId == id
                       select d;
            //var TEMPlIST = objEntities.Subjects.ToList();
            CourseViewModel courseView = new CourseViewModel
            {
                CourseId = Convert.ToInt32(courses.CourseId),
                CourseName = courses.CourseName
            };
            if (courses == null)
            {
                return HttpNotFound();
            }
            return View(courseView);
        }
        /// <summary>
        /// method for deletting course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteCourse(int id)

        {
            try
            {
                Courses courses = objEntities.Courses.Find(id);
                objEntities.Courses.Remove(courses);
                objEntities.SaveChanges();
                return RedirectToAction("GetCourse");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: delete failed", ex.Message);
                return View(ex);
            }
        }
        /// <summary>
        /// logout
        /// </summary>
        /// <returns></returns>




    }
}