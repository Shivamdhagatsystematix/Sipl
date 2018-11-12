
using Sipl.App_Start;
using Sipl.DataBase;
using Sipl.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sipl.Controllers
{
    public class NetUserViewController : Controller
    {
        SiplDatabaseEntities objEntities = new SiplDatabaseEntities();
        /// <summary>
        /// List of all Registered User 
        /// </summary>
        /// <returns></returns>
        public ActionResult RegisteredUserList()
        {
            // Get user roles from DB
            ViewBag.Role = new SelectList(objEntities.NetRoles.ToList(), "RoleId", "RoleName");
            List<NetUserViewModel> objNetUserViewModel = new List<NetUserViewModel>();

            //Get List of users from DB
            var data = (from p in objEntities.NetUsers select p).ToList();
            foreach (var item in data)
            {
                //To get Address From DB
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
        /// Details of Registered Users
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult RegisteredUserDetails(int? id)
        {
            NetUserViewModel objNetUserViewModel = new NetUserViewModel();
            try
            {
                ViewBag.SendId = id;

                var getUser = (from d in objEntities.Address
                               join c in objEntities.NetUsers on d.UserId equals c.UserId
                               join s in objEntities.UserRole on c.UserId equals s.UserId
                               join e in objEntities.SubjectInCourse on c.CourseId equals e.CourseId
                               where d.UserId == id

                               select new NetUserViewModel
                               {
                                   UserId = c.UserId,
                                   FirstName = c.FirstName,
                                   LastName = c.LastName,
                                   Password = c.Password,
                                   RoleId = s.RoleId,
                                   Gender = c.Gender,
                                   DOB = c.DOB,
                                   Email = c.Email,
                                   IsVerified = c.IsVerified,
                                   IsActive = c.IsActive,
                                   DateCreated = c.DateCreated,
                                   CourseName = c.Courses.CourseName,
                                   CurrentAddress = d.CurrentAddress,
                                   PermanantAddress = d.PermanantAddress,
                                   Country = d.Countries.CountryName,
                                   States = d.States.StateName,
                                   Cities = d.Cities.CityName,
                                   Pincode = d.Pincode
                               }).FirstOrDefault();
                return View(getUser);
            }
            catch (Exception er)
            {
                Console.Write(er.Message);
                return View();
            }
        }
        public ActionResult TeacherDetails(int? id)
        {
            NetUserViewModel objNetUserViewModel = new NetUserViewModel();
            try
            {

                var getUser = (from d in objEntities.Address
                               join c in objEntities.NetUsers on d.UserId equals c.UserId
                               join s in objEntities.UserRole on c.UserId equals s.UserId
                               join f in objEntities.TeacherInSubject on c.UserId equals f.UserId
                               join su in objEntities.Subjects on f.SubjectId equals su.SubjectId
                               where d.UserId == id

                               select new NetUserViewModel
                               {
                                   UserId = c.UserId,
                                   FirstName = c.FirstName,
                                   LastName = c.LastName,
                                   Password = c.Password,
                                   RoleId = s.RoleId,
                                   Gender = c.Gender,
                                   DOB = c.DOB,
                                   Email = c.Email,
                                   IsVerified = c.IsVerified,
                                   IsActive = c.IsActive,
                                   DateCreated = c.DateCreated,
                                   SubjectName = su.SubjectName,
                                   CurrentAddress = d.CurrentAddress,
                                   PermanantAddress = d.PermanantAddress,
                                   Country = d.Countries.CountryName,
                                   States = d.States.StateName,
                                   Cities = d.Cities.CityName,
                                   Pincode = d.Pincode
                               }).FirstOrDefault();
                return View(getUser);
            }
            catch (Exception er)
            {
                Console.Write(er.Message);
                return View();
            }
        }

        /// <summary>
        ///GET :Registration Method For Users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RegisterUser()
        {
            //TO GET ROLES FROM DATABASE
            var roles = (from b in objEntities.NetRoles select b).ToList();
            var model = new NetUserViewModel
            {
                Role = roles.Select(x => new SelectListItem
                {
                    Value = x.RoleId,
                    Text = x.RoleName
                }).ToList()
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
                List<SelectListItem> listCountry = new List<SelectListItem>();
                List<SelectListItem> listState = new List<SelectListItem>();
                List<SelectListItem> listCity = new List<SelectListItem>();


                foreach (var m in country)
                {
                    listCountry.Add(new SelectListItem { Text = m.CountryName, Value = m.CountryId.ToString() });
                }
                ViewBag.country = listCountry;
                ViewBag.State = listState;
                ViewBag.City = listCity;
            }
            return View(model);
        }

        /// <summary>
        /// Post: Register Method For Users
        /// </summary>
        /// <param name="objNetUserViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterUser(NetUserViewModel objNetUserViewModel)
        {
            using (var dbTransaction = objEntities.Database.BeginTransaction())
            {
                ViewBag.Role = new SelectList(objEntities.NetRoles.ToList(), "RoleId", "RoleName");
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
                            Pincode = objNetUserViewModel.Pincode,
                            UserId = userId
                        };
                        objEntities.Address.Add(objAddress);
                        objEntities.SaveChanges();
                        dbTransaction.Commit();
                        ModelState.Clear();
                        return RedirectToAction("RegisteredUser", "LogIn");
                    }
                    return View(objNetUserViewModel);
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    Console.WriteLine("Exception source: {0} user is failed to register", ex.Message);
                    return View(ex);
                }
            }
        }

        /// <summary>
        /// Update Method for all users
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult UpdateRegisteredUsers(int? id)
        {
            CrudViewModel objCrudViewModel = new CrudViewModel();
            try
            {
                //TO GET ROLES FROM DATABASE
                var roles = (from b in objEntities.NetRoles select b).ToList();
                List<SelectListItem> listRoles = new List<SelectListItem>();
                foreach (var x in roles)
                {
                    listRoles.Add(new SelectListItem
                    {
                        Value = x.RoleId,
                        Text = x.RoleName
                    });
                }
                ViewBag.roles = listRoles;

                //GET : COURSE FOR USERS
                var course = (from b in objEntities.Courses select b).ToList();

                List<SelectListItem> listCourse = new List<SelectListItem>();
                foreach (var x in course)
                {
                    listCourse.Add(new SelectListItem
                    {
                        Value = x.CourseId.ToString(),
                        Text = x.CourseName
                    });
                }
                ViewBag.course = listCourse;

                //TO GET COUNTRY ,STATES AND CITY
                var country = objEntities.Countries.ToList();
                var states = objEntities.States.ToList();
                var cities = objEntities.Cities.ToList();
                List<SelectListItem> listCountry = new List<SelectListItem>();
                List<SelectListItem> listState = new List<SelectListItem>();
                List<SelectListItem> listCity = new List<SelectListItem>();


                foreach (var m in country)
                {
                    listCountry.Add(new SelectListItem { Text = m.CountryName, Value = m.CountryId.ToString() });
                }
                foreach (var m in states)
                {
                    listState.Add(new SelectListItem { Text = m.StateName, Value = m.StateId.ToString() });
                }
                foreach (var m in cities)
                {
                    listCity.Add(new SelectListItem { Text = m.CityName, Value = m.CityId.ToString() });
                }
                ViewBag.country = listCountry;
                ViewBag.state = listState;
                ViewBag.city = listCity;

                var getUser = (from d in objEntities.Address
                               join c in objEntities.NetUsers on d.UserId equals c.UserId
                               join s in objEntities.UserRole on c.UserId equals s.UserId
                               where d.UserId == id

                               select new CrudViewModel
                               {
                                   UserId = c.UserId,
                                   FirstName = c.FirstName,
                                   LastName = c.LastName,
                                   Password = c.Password,
                                   Id = s.Id,
                                   AddressId = d.AddressId,
                                   RoleId = s.RoleId,
                                   Gender = c.Gender,
                                   DOB = c.DOB,
                                   Email = c.Email,
                                   IsVerified = c.IsVerified,
                                   IsActive = c.IsActive,
                                   DateCreated = c.DateCreated,
                                   CourseId = c.CourseId,
                                   CurrentAddress = d.CurrentAddress,
                                   PermanantAddress = d.PermanantAddress,
                                   CountryId = d.CountryId,
                                   StateId = d.StateId,
                                   CityId = d.CityId,
                                   Pincode = d.Pincode
                               }).FirstOrDefault();

                return View(getUser);
            }
            catch (Exception er)
            {
                Console.Write(er.Message);
                return View();
            }
        }


        /// <summary>
        /// Update Method for all users
        /// </summary>
        /// <param name="objNetUserViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateRegisteredUsers(CrudViewModel objCrudViewModel)

        {
            ViewBag.Role = new SelectList(objEntities.NetRoles.ToList(), "RoleId", "RoleName");
            try
            {
                if (ModelState.IsValid)
                    {
                        // to specify UserRole according to their UserId
                        UserRole objUserRole = new UserRole
                        {
                            Id = objCrudViewModel.Id,
                            RoleId = objCrudViewModel.RoleId,
                            UserId = objCrudViewModel.UserId
                        };
                        objEntities.Entry(objUserRole).State = EntityState.Modified;
                        objEntities.UserRole.Add(objUserRole);
                        objEntities.SaveChanges();

                        //Encrytion For Password
                        //Encrytion For Password
                        var keyNew = "Test";
                        var password = Helper.EncodePassword(objCrudViewModel.Password, keyNew);

                        NetUsers objNetUsers = new NetUsers
                        {
                            UserId = objCrudViewModel.UserId,
                            FirstName = objCrudViewModel.FirstName,
                            LastName = objCrudViewModel.LastName,
                            Gender = objCrudViewModel.Gender,
                            CourseId = objCrudViewModel.CourseId,
                            Email = objCrudViewModel.Email,
                            Password = objCrudViewModel.Password,
                            DOB = objCrudViewModel.DOB,
                            IsActive = objCrudViewModel.IsActive,
                            IsVerified = objCrudViewModel.IsVerified,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                        };
                        objEntities.Entry(objNetUsers).State = EntityState.Modified;
                        objEntities.SaveChanges();
                        //to get userId
                        var userId = objNetUsers.UserId;

                        //to add data in Address Table
                        Address objAddress = new Address
                        {
                            AddressId = objCrudViewModel.AddressId,
                            CountryId = objCrudViewModel.CountryId,
                            StateId = objCrudViewModel.StateId,
                            CityId = objCrudViewModel.CityId,
                            CurrentAddress = objCrudViewModel.CurrentAddress,
                            PermanantAddress = objCrudViewModel.PermanantAddress,
                            Pincode = objCrudViewModel.Pincode,
                            UserId = userId
                        };
                        objEntities.Entry(objAddress).State = EntityState.Modified;
                        objEntities.SaveChanges();
                        if (Convert.ToInt32(Session["RoleId"]) == 2)
                        {
                            return RedirectToAction("UserProfile", "Admin/TeacherInfo", new { id = userId });
                            //return RedirectToAction("StudentProfile", , Id = userId }));
                        }
                        else if (Convert.ToInt32(Session["RoleId"]) == 1)
                        {
                            return RedirectToAction("TeacherProfile", "Admin/TeacherInfo", new { id = userId });
                        }
                        else if (Convert.ToInt32(Session["RoleId"]) == 3)
                                {
                            return RedirectToAction("UserSearchView", "Admin/TeacherInfo", new { id = userId });

                        }
                    }
                    return View(objCrudViewModel);

                }

            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0} user is failed to Update", ex.Message);
                return View(ex);
            }
        }

        /// <summary>
        /// Get Delete Method for all users
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteRegisteredUsers(int? id)
        {
            CrudViewModel objCrudViewModel = new CrudViewModel();
            try
            {
                //TO GET ROLES FROM DATABASE
                var roles = (from b in objEntities.NetRoles select b).ToList();

                List<SelectListItem> listRoles = new List<SelectListItem>();
                foreach (var x in roles)
                {
                    listRoles.Add(new SelectListItem
                    {
                        Value = x.RoleId,
                        Text = x.RoleName
                    });
                }
                ViewBag.roles = listRoles;

                //GET : COURSE FOR USERS
                var course = (from b in objEntities.Courses select b).ToList();
                List<SelectListItem> listCourse = new List<SelectListItem>();
                foreach (var x in course)
                {
                    listCourse.Add(new SelectListItem
                    {
                        Value = x.CourseId.ToString(),
                        Text = x.CourseName
                    });
                }
                ViewBag.course = listCourse;

                //TO GET COUNTRY ,STATES AND CITY
                var country = objEntities.Countries.ToList();
                var states = objEntities.States.ToList();
                var cities = objEntities.Cities.ToList();
                List<SelectListItem> listCountry = new List<SelectListItem>();
                List<SelectListItem> listState = new List<SelectListItem>();
                List<SelectListItem> listCity = new List<SelectListItem>();


                foreach (var m in country)
                {
                    listCountry.Add(new SelectListItem { Text = m.CountryName, Value = m.CountryId.ToString() });
                }
                foreach (var m in states)
                {
                    listState.Add(new SelectListItem { Text = m.StateName, Value = m.StateId.ToString() });
                }
                foreach (var m in cities)
                {
                    listCity.Add(new SelectListItem { Text = m.CityName, Value = m.CityId.ToString() });
                }
                ViewBag.country = listCountry;
                ViewBag.state = listState;
                ViewBag.city = listCity;

                var getUser = (from d in objEntities.Address
                               join c in objEntities.NetUsers on d.UserId equals c.UserId
                               join s in objEntities.UserRole on c.UserId equals s.UserId
                               where d.UserId == id

                               select new CrudViewModel
                               {
                                   UserId = c.UserId,
                                   FirstName = c.FirstName,
                                   LastName = c.LastName,
                                   Password = c.Password,
                                   Id = s.Id,
                                   AddressId = d.AddressId,
                                   RoleId = s.RoleId,
                                   Gender = c.Gender,
                                   DOB = c.DOB,
                                   Email = c.Email,
                                   IsVerified = c.IsVerified,
                                   IsActive = c.IsActive,
                                   DateCreated = c.DateCreated,
                                   CourseId = c.CourseId,
                                   CurrentAddress = d.CurrentAddress,
                                   PermanantAddress = d.PermanantAddress,
                                   CountryId = d.CountryId,
                                   StateId = d.StateId,
                                   CityId = d.CityId,
                                   Pincode = d.Pincode
                               }).FirstOrDefault();
                return View(getUser);
            }
            catch (Exception er)
            {
                Console.Write(er.Message);
                return View();
            }
        }

        /// <summary>
        /// POST: Delete Method for all users
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRegisteredUsers(byte id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    NetUsers netUsers = objEntities.NetUsers.Find(id);
                    objEntities.NetUsers.Remove(netUsers);

                    objEntities.SaveChanges();
                }
                return RedirectToAction("UserSearchView", "Admin/TeacherInfo");
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
        public JsonResult GetState(int id)
        {
            var states = objEntities.States.Where(x => x.CountryId == id).ToList();
            List<SelectListItem> listates = new List<SelectListItem>();
            listates.Add(new SelectListItem { Text = "Select State", Value = "0" });
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
        public JsonResult GetCity(int id)
        {
            var city = objEntities.Cities.Where(x => x.StateId == id).ToList();
            List<SelectListItem> licity = new List<SelectListItem>();
            licity.Add(new SelectListItem { Text = "Select City", Value = "0" });
            if (city != null)
            {
                foreach (var l in city)
                {
                    licity.Add(new SelectListItem { Text = l.CityName, Value = l.CityId.ToString() });
                }
            }
            return Json(new SelectList(licity, "Value", "Text", JsonRequestBehavior.AllowGet));
        }

        /// <summary>
        /// To Check Existing Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public JsonResult CheckEmailExists(string email)
        {
            try
            {
                bool isValid = !objEntities.NetUsers.ToList().Exists(p => p.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
                return Json(isValid, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Source);
                return null;
            }
        }
    }
}




