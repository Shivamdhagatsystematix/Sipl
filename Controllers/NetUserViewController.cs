using Sipl.DataBase;
using Sipl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sipl.Controllers
{
    public class NetUserViewController : Controller

    {
        SipDatabaseEntities objEntities = new SipDatabaseEntities();

        // GET: NetUserView
        public ActionResult Index()
        {
            SipDatabaseEntities objEntities = new SipDatabaseEntities();
            List<NetUserViewModel> objNetUserViewModel = new List<NetUserViewModel>();
            var data = (from p in objEntities.NetUsers select p).ToList();
            foreach (var item in data)
            {
                NetUserViewModel netUser = new NetUserViewModel
                {
                    UserId = item.UserId,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Gender = item.Gender,
                    Email = item.Email,
                    Password = item.Password,
                    ConfirmPassword = item.ConfirmPassword,
                    DOB = item.DOB,
                    IsActive = item.IsActive,
                    DateCreated = item.DateCreated,
                    DateModified = item.DateModified
                };

                objNetUserViewModel.Add(netUser);

            };
            return View(objNetUserViewModel);
        }

        // GET: NetUserView/Details/5 
        public ActionResult  Details (int ?id)
        {
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                NetUsers netUsers = objEntities.NetUsers.Find(id);
                var data = from d in objEntities.NetUsers
                                   where d.UserId == id
                                   select d;
                var TEMPlIST = objEntities.NetUsers.ToList();

                NetUserViewModel netUser = new NetUserViewModel
                {
                    UserId = netUsers.UserId,
                    FirstName = netUsers.FirstName,
                    LastName = netUsers.LastName,
                    Gender = netUsers.Gender,
                    Email = netUsers.Email,
                    Password = netUsers.Password,
                    ConfirmPassword = netUsers.ConfirmPassword,
                    DOB = netUsers.DOB,
                    IsActive = netUsers.IsActive,
                    DateCreated = netUsers.DateCreated,
                    DateModified = netUsers.DateModified
                };

               

            if (netUsers == null)
            {
                return HttpNotFound();
            }
            return View(netUser);
        }
    }

        // GET: NetUserView/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NetUserView/Create
       
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create([Bind(Include = "UserId,FirstName,LastName," +
            "Role,Gender,Email,Password,ConfirmPassword,DOB" +
            ",IsActive,DateCreated,DateModified")] NetUsers netUsers)
        {
            try
            {
                NetUserViewModel netUser = new NetUserViewModel
                    

                {
                    if (ModelState.IsValid)
                    {
                        db.NetUsers.Add(netUsers);
                       db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                 


                    return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View(netUsers);
            }
        }

        // GET: NetUserView/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: NetUserView/Edit/5
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

        // GET: NetUserView/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: NetUserView/Delete/5
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
