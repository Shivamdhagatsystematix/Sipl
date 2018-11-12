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
    public class SubjectController : Controller
    {
        SiplDatabaseEntities objEntities = new SiplDatabaseEntities();
        /// <summary>
        ///  List of all Subjects
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSubject()
        {
            List<SubjectViewModel> objSubjectViewModel = new List<SubjectViewModel>();
            var data = (from p in objEntities.Subjects select p).ToList();
            foreach (var item in data)
            {
                SubjectViewModel subject = new SubjectViewModel
                {
                    SubjectId = item.SubjectId,
                    SubjectName = item.SubjectName,
                    
                };
                objSubjectViewModel.Add(subject);
            };
            return View(objSubjectViewModel);
        }

        /// <summary>
        ///  GET :Admin/Subject/To Create Subject
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateSubject()
        {
            return View();
        }

        /// <summary>
        /// POST : Admin/Subject/To Create Subject
        /// </summary>
        /// <param name="objSubjectViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSubject(SubjectViewModel objSubjectViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Subjects objSubjects = new Subjects
                    {
                        SubjectName = objSubjectViewModel.SubjectName
                    };
                    objEntities.Subjects.Add(objSubjects);
                    objEntities.SaveChanges();
                    return RedirectToAction("GetSubject");
                };
                return View(objSubjectViewModel);
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// GET :Admin/Subject/Edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditSubject(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subjects subjects = objEntities.Subjects.Find(id);
            var data = from d in objEntities.Subjects
                       where d.SubjectId == id
                       select d;
            //var TEMPlIST = objEntities.Subjects.ToList();
            SubjectViewModel subjectView = new SubjectViewModel
            {
                SubjectId = subjects.SubjectId,
                SubjectName = subjects.SubjectName
            };
            if (subjects == null)
            {
                return HttpNotFound();
            }
            return View(subjectView);
        }

        /// <summary>
        ///  POST :Admin/Subject/Edit
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditSubject(int id, Subjects objSubjects)

        {
            {
                if (ModelState.IsValid)

                {
                    Subjects objSubject = new Subjects
                    {
                        SubjectId = id,
                        SubjectName = objSubjects.SubjectName

                    };
                    objEntities.Entry(objSubject).State = EntityState.Modified;
                    objEntities.SaveChanges();
                    return RedirectToAction("GetSubject");
                }
                return View(objSubjects);
            }

        }
        /// <summary>
        /// Admin/Subject/Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteSubject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subjects subjects = objEntities.Subjects.Find(id);
            var data = from d in objEntities.Subjects
                       where d.SubjectId == id
                       select d;
            //var TEMPlIST = objEntities.Subjects.ToList();
            SubjectViewModel subjectView = new SubjectViewModel
            {
                SubjectId = subjects.SubjectId,
                SubjectName = subjects.SubjectName
            };
            if (subjects == null)
            {
                return HttpNotFound();
            }
            return View(subjectView);
        }

        /// <summary>
        /// Admin/Subject/Delete
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteSubject(int id, SubjectViewModel objSubjectViewModel)
     
        {
            try
            {
               


                Subjects subjects = objEntities.Subjects.Find(id);
                objEntities.Subjects.Remove(subjects);
                objEntities.SaveChanges();
             

                return RedirectToAction("GetSubject");
            }
            catch (Exception ex)
            {
             Console.WriteLine("Exception source: {0} user is failed to register", ex.Message);
                return View(ex);
            }

        }
   
    }
}
