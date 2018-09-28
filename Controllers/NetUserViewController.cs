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
                    Role =item.Role,
                    Gender = item.Gender,
                    Email = item.Email,
                    Password = item.Password,
                    ConfirmPassword = item.ConfirmPassword,
                    DOB = item.DOB,
                    IsActive= item.IsActive,
              
                    DateCreated = item.DateCreated
              
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
                    Role= netUsers.Role,
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

       [HttpPost]
       [ValidateAntiForgeryToken]
        public ActionResult Create(NetUserViewModel objNetUserViewModel)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    NetUsers objNetUsers = new NetUsers
                    {
                        
                        FirstName = objNetUserViewModel.FirstName,
                        LastName = objNetUserViewModel.LastName,
                        Role= objNetUserViewModel.Role,
                         Gender = objNetUserViewModel.Gender,
                        Email = objNetUserViewModel.Email,
                        Password = objNetUserViewModel.Password,
                        ConfirmPassword = objNetUserViewModel.ConfirmPassword,
                        DOB = objNetUserViewModel.DOB,
                        IsActive= objNetUserViewModel.IsActive,
                        DateCreated =DateTime.Now,
                        DateModified = DateTime.Now
                    };

                    objEntities.NetUsers.Add(objNetUsers);
                
                    objEntities.SaveChanges();
                    return RedirectToAction("Index");

                }

                return View(objNetUserViewModel);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // GET: NetUserView/Edit/5
        public ActionResult Edit(int id)
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
                    //UserId = netUsers.UserId,
                    FirstName = netUsers.FirstName,
                    LastName = netUsers.LastName,
                    Role = netUsers.Role,
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

        // POST: NetUserView/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NetUserViewModel objNetUserViewModel)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    NetUsers objNetUsers = new NetUsers
                    {

                        FirstName = objNetUserViewModel.FirstName,
                        LastName = objNetUserViewModel.LastName,
                        Role = objNetUserViewModel.Role,
                        Gender = objNetUserViewModel.Gender,
                        Email = objNetUserViewModel.Email,
                        Password = objNetUserViewModel.Password,
                        ConfirmPassword = objNetUserViewModel.ConfirmPassword,
                        DOB = objNetUserViewModel.DOB,
                        IsActive = objNetUserViewModel.IsActive,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now
                    };

                    objEntities.NetUsers.Add(objNetUsers);

                    objEntities.SaveChanges();
                    return RedirectToAction("Index");

                }

                return View(objNetUserViewModel);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // GET: NetUserView/Delete/5
        public ActionResult Delete(int id)
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
                    //UserId = netUsers.UserId,
                    FirstName = netUsers.FirstName,
                    LastName = netUsers.LastName,
                    Role = netUsers.Role,
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
        // POST: NetUserView/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete( byte id)
        {
            try
            {

                if (ModelState.IsValid)
                {

                    NetUsers netUsers = objEntities.NetUsers.Find(id);
                    objEntities.NetUsers.Remove(netUsers);

                    objEntities.SaveChanges();
                   

                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
