using Sipl.Areas.Admin.Models;
using Sipl.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sipl.Areas.Admin.Controllers
{
    public class CourseController : Controller
    {
        SiplDatabaseEntities objEntities = new SiplDatabaseEntities();

        public ActionResult Index()
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


        //get
        public ActionResult Create()
        {
            //ViewBag.SubjectId = new SelectList(objEntities.Subjects,
            //           "SubjectId", "SubjectName");
            //return View();

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseViewModel objCourseViewModel)
        {
            //ViewBag.Subject = new SelectList(objEntities.Subjects.ToList(), "SubjectId", "SubjectName");
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
                    var courseId = objCourses.CourseId;

                    SubjectInCourse objSubjectInCourse = new SubjectInCourse
                    {
                        SubjectId = objCourseViewModel.SubjectId,
                        CourseId = courseId
                    };
                    objEntities.SubjectInCourse.Add(objSubjectInCourse);
                    objEntities.SaveChanges();
                };



                return RedirectToAction("Index","Course", objCourseViewModel);
            }

            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}