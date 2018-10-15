using Sipl.Areas.Admin.Models;
using Sipl.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult CreateCourse()
        {
            var Subjects = (from b in objEntities.Subjects select b).ToList();
            var model = new CourseViewModel
            {
                Subject = Subjects.Select(x => new SelectListItem
                {
                    Value = x.SubjectId.ToString(),
                    Text = x.SubjectName
                })
            };
            return View(model);
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
                        SubjectId = objCourseViewModel.SubjectId
                    };
                    var test = objEntities.Courses.Add(objCourses);
                    objEntities.SaveChanges();
                    //To get CourseId
                    var courseId = objCourses.CourseId;

                    //To add Respective Subject in Course in SubjectInCourse Table
                    SubjectInCourse objSubjectInCourse = new SubjectInCourse
                    {
                        SubjectId = objCourseViewModel.SubjectId,
                        CourseId = courseId
                    };
                    objEntities.SubjectInCourse.Add(objSubjectInCourse);
                    objEntities.SaveChanges();
                };
                return RedirectToAction("GetCourse", "Course", objCourseViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}