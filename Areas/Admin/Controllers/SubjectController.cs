using Sipl.Areas.Admin.Models;
using Sipl.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult Index()
        {
            List<SubjectViewModel> objSubjectViewModel = new List<SubjectViewModel>();
            var data = (from p in objEntities.Subjects select p).ToList();
            foreach (var item in data)
            {
                SubjectViewModel subject = new SubjectViewModel
                {
                    SubjectId = item.SubjectId,
                    SubjectName = item.SubjectName
                };
                objSubjectViewModel.Add(subject);
            };
            return View(objSubjectViewModel);
        }

       
        /// <summary>
        ///  Admin/Subject/To Create Subject
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

       
        /// <summary>
        ///  Admin/Subject/To Create Subject
        /// </summary>
        /// <param name="objSubjectViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SubjectViewModel objSubjectViewModel)
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
                    return RedirectToAction("Index");
                };
                return View(objSubjectViewModel);
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Admin/Subject/Edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subjects subjects = objEntities.Subjects.Find(id);
            var data = from d in objEntities.Subjects
                       where d.SubjectId == id
                       select d;
            var TEMPlIST = objEntities.Subjects.ToList();

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
        ///  Admin/Subject/Edit
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Admin/Subject/Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: 
        /// <summary>
        /// Admin/Subject/Delete
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
