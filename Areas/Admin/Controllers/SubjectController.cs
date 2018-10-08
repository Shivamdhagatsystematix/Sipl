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
    public class SubjectController : Controller
    {
        SiplDatabaseEntities objEntities = new SiplDatabaseEntities();
        // GET: Admin/Subject
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

        // GET: Admin/Subject/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Subject/Create
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

        // GET: Admin/Subject/Edit/5
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

        // POST: Admin/Subject/Edit/5
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

        // GET: Admin/Subject/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Subject/Delete/5
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
