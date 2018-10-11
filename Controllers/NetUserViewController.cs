
using Sipl.App_Start;
using Sipl.DataBase;
using Sipl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Sipl.Controllers
{

    public class NetUserViewController : Controller

    {
        SiplDatabaseEntities objEntities = new SiplDatabaseEntities();


        /// <summary>
        /// Index of all Registered User 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()

        {
            ViewBag.Role = new SelectList(objEntities.NetRoles.ToList(), "RoleId", "RoleName");
            List<NetUserViewModel> objNetUserViewModel = new List<NetUserViewModel>();
            var data = (from p in objEntities.NetUsers select p).ToList();
            foreach (var item in data)
            {
                var userAddressInfo = (from p in objEntities.Address where p.UserId == item.UserId select p).FirstOrDefault();

                if (userAddressInfo != null)
                {
                    NetUserViewModel netUser = new NetUserViewModel
                    {
                        UserId = item.UserId,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Gender = item.Gender,
                        Email = item.Email,
                        Password = item.Password,
                        DOB = item.DOB,
                        IsActive = item.IsActive,
                        CurrentAddress = userAddressInfo.CurrentAddress,
                        PermanantAddress = userAddressInfo.PermanantAddress,
                        Country = userAddressInfo.Countries.CountryName,
                        States = userAddressInfo.States.StateName,
                        Cities = userAddressInfo.Cities.CityName,
                        DateCreated = item.DateCreated

                    };
                    objNetUserViewModel.Add(netUser);
                }

            };
            return View(objNetUserViewModel);
        }


       /// <summary>
       /// Details of User
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
        public ActionResult Details(int? id)
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
                    DOB = netUsers.DOB,
                    IsActive = netUsers.IsActive,
                    DateCreated = netUsers.DateCreated,
                    DateModified = netUsers.DateModified
                };
                return View(netUser);
            }

        }

        /// <summary>
        ///Register Method For Users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RegisterUser()
        {

            //TO GET ROLES FROM DATABASE
            var Roles = (from b in objEntities.NetRoles select b).ToList();
            var model = new NetUserViewModel
            {
                Role = Roles.Select(x => new SelectListItem
                {
                    Value = x.RoleId,
                    Text = x.RoleName
                })
            };

            //GET : COURSE FOR USERS
            var course = (from b in objEntities.Courses select b).ToList();
            model.Course = course.Select(x => new SelectListItem
            {
                Value = x.CourseId.ToString(),
                Text = x.CourseName
            });

            //TO GET COUNTRY ,STATES AND CITY
            {
                var country = objEntities.Countries.ToList();
                List<SelectListItem> liCountry = new List<SelectListItem>();
                List<SelectListItem> liState = new List<SelectListItem>();
                List<SelectListItem> liCity = new List<SelectListItem>();

                liCountry.Add(new SelectListItem { Text = "", Value = "0" });
                liState.Add(new SelectListItem { Text = "", Value = "0" });
                liCity.Add(new SelectListItem { Text = "", Value = "0" });

                foreach (var m in country)
                {
                    liCountry.Add(new SelectListItem { Text = m.CountryName, Value = m.CountryId.ToString() });
                }
                ViewBag.country = liCountry;
                ViewBag.State = liState;
                ViewBag.City = liCity;

            }
            return View(model);
        }

        /// <summary>
        /// Register Method For Users
        /// </summary>
        /// <param name="objNetUserViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterUser(NetUserViewModel objNetUserViewModel)
        {
            ViewBag.Role = new SelectList(objEntities.NetRoles.ToList(), "RoleId", "RoleName");
            //ViewBag.Role = new SelectList(objEntities.countries.ToList(), "", "RoleName");
            try
            {
                if (ModelState.IsValid)
                {
                    //Encrytion For Password
                    var keyNew = "Test";
                    var password = Helper.EncodePassword(objNetUserViewModel.Password, keyNew);
                    NetUsers objNetUsers = new NetUsers
                    {
                        FirstName = objNetUserViewModel.FirstName,
                        LastName = objNetUserViewModel.LastName,
                        Gender = objNetUserViewModel.Gender,
                        CourseId = objNetUserViewModel.CourseId,
                        Email = objNetUserViewModel.Email,
                        Password = password,
                        DOB = objNetUserViewModel.DOB,
                        IsActive = objNetUserViewModel.IsActive,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                    };

                    objEntities.NetUsers.Add(objNetUsers);
                    objEntities.SaveChanges();
                    //to get userId
                    var userId = objNetUsers.UserId;

                    // to specify UserRole according to their UserI
                    UserRole objUserRole = new UserRole
                    {
                        RoleId = objNetUserViewModel.RoleId,
                        UserId = userId
                    };
                    objEntities.UserRole.Add(objUserRole);
                    objEntities.SaveChanges();


                    //to add data in Address Table
                    Address objAddress = new Address
                    {
                        CountryId = objNetUserViewModel.CountryId,
                        StateId = objNetUserViewModel.StateId,
                        CityId = objNetUserViewModel.CityId,
                        CurrentAddress = objNetUserViewModel.CurrentAddress,
                        PermanantAddress = objNetUserViewModel.PermanantAddress,
                        UserId = userId

                    };
                    objEntities.Address.Add(objAddress);
                    objEntities.SaveChanges();
                    ModelState.Clear();
                    return RedirectToAction("RegisteredUser", "LogIn");
                }
                return View(objNetUserViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update Method for all users
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            ViewBag.Role = new SelectList(objEntities.NetRoles.ToList(), "RoleId", "RoleName");
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
                    FirstName = netUsers.FirstName,
                    LastName = netUsers.LastName,
                    Gender = netUsers.Gender,
                    Email = netUsers.Email,
                    Password = netUsers.Password,
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

        /// <summary>
        /// Update Method for all users
        /// </summary>
        /// <param name="objNetUserViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NetUserViewModel objNetUserViewModel)
        {
            ViewBag.Role = new SelectList(objEntities.NetRoles.ToList(), "RoleId", "RoleName");
            try
            {
                if (ModelState.IsValid)
                {
                    NetUsers objNetUsers = new NetUsers
                    {

                        FirstName = objNetUserViewModel.FirstName,
                        LastName = objNetUserViewModel.LastName,
                        Gender = objNetUserViewModel.Gender,
                        Email = objNetUserViewModel.Email,
                        Password = objNetUserViewModel.Password,
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

        /// <summary>
        ///  Delete Method for all users
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

                    FirstName = netUsers.FirstName,
                    LastName = netUsers.LastName,
                    Gender = netUsers.Gender,
                    Email = netUsers.Email,
                    Password = netUsers.Password,
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

        /// <summary>
        /// POST: Delete Method for all users
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(byte id)
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

        /// <summary>
        ///  Method For Getting Respective States Accordind to their Country
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult getstate(int id)
        {
            var states = objEntities.States.Where(x => x.CountryId == id).ToList();
            List<SelectListItem> listates = new List<SelectListItem>();
            listates.Add(new SelectListItem { Text = "", Value = "0" });

            if (states != null)
            {
                foreach (var x in states)
                {
                    listates.Add(new SelectListItem { Text = x.StateName, Value = x.StateId.ToString() });
                }
            }
            return Json(new SelectList(listates, "Value", "Text", JsonRequestBehavior.AllowGet));
        }

        /// <summary>
        ///  Method For Getting Respective Cities Accordind to their States
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult getCity(int id)
        {
            var city = objEntities.Cities.Where(x => x.StateId == id).ToList();
            List<SelectListItem> licity = new List<SelectListItem>();

            licity.Add(new SelectListItem { Text = "", Value = "0" });
            if (city != null)
            {
                foreach (var l in city)
                {
                    licity.Add(new SelectListItem { Text = l.CityName, Value = l.CityId.ToString() });
                }
            }

            return Json(new SelectList(licity, "Value", "Text", JsonRequestBehavior.AllowGet));
        }
    }
}




