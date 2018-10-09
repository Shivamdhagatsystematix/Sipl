
using Sipl.App_Start;
using Sipl.DataBase;
using Sipl.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using static Sipl.Models.NetUserViewModel;

namespace Sipl.Controllers
{
    [Authorize]
    public class NetUserViewController : Controller

    {
        SiplDatabaseEntities objEntities = new SiplDatabaseEntities();


        // GET: NetUserView
      
        public ActionResult Index()

        {
            ViewBag.Role = new SelectList(objEntities.NetRoles.ToList(), "RoleId", "RoleName");



            List<NetUserViewModel> objNetUserViewModel = new List<NetUserViewModel>();
            var data = (from p in objEntities.NetUsers select p).ToList();
            foreach (var item in data)
            {
                var userAddressInfo = (from p in objEntities.Address where p.UserId == item.UserId select p).FirstOrDefault();
                //var userCourseInfo = (from p in objEntities.Courses where p.CourseId == item.CourseId select p).FirstOrDefault();
                //.FirstOrDefault(u => u.UserId == item.UserId);
                if (userAddressInfo != null)
                {
                    NetUserViewModel netUser = new NetUserViewModel
                    {
                        UserId = item.UserId,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        //Role =item.Role,
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


        // GET: NetUserView/Details/5 
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
                    //Role= netUsers.RoleId,
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


        // GET: NetUserView/Create
        public ActionResult Create()
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


        // POST: NetUserView/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NetUserViewModel objNetUserViewModel)
        {
            ViewBag.Role = new SelectList(objEntities.NetRoles.ToList(), "RoleId", "RoleName");
            //ViewBag.Role = new SelectList(objEntities.countries.ToList(), "", "RoleName");
            try
            {


                if (ModelState.IsValid)
                {
                    NetUsers objNetUsers = new NetUsers
                    {


                        FirstName = objNetUserViewModel.FirstName,
                        LastName = objNetUserViewModel.LastName,
                        Gender = objNetUserViewModel.Gender,
                        CourseId = objNetUserViewModel.CourseId,
                        Email = objNetUserViewModel.Email,
                        Password = objNetUserViewModel.Password,
                        //ConfirmPassword = objNetUserViewModel.ConfirmPassword,
                        DOB = objNetUserViewModel.DOB,
                        IsActive = objNetUserViewModel.IsActive,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,

                    };


                    {
                        var keyNew = Helper.GeneratePassword(10);
                        var password = Helper.EncodePassword(objNetUserViewModel.Password, keyNew);
                        objNetUserViewModel.Password = password;

                        objNetUserViewModel.Password = keyNew;
                        objEntities.NetUsers.Add(objNetUsers);
                        objEntities.SaveChanges();
                        ModelState.Clear();

                    }
                    ViewBag.ErrorMessage = "User Allredy Exixts!!!!!!!!!!";



                    var test = objEntities.NetUsers.Add(objNetUsers);
                    objEntities.SaveChanges();
                    var userId = objNetUsers.UserId;


                    UserRole objUserRole = new UserRole
                    {
                        RoleId = objNetUserViewModel.RoleId,
                        UserId = userId
                    };
                    objEntities.UserRole.Add(objUserRole);
                    objEntities.SaveChanges();



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

                    //  objUserRole.UserRole.Add(objUserRole);
                    objEntities.SaveChanges();



                    return RedirectToAction("RegisteredUser", "LogIn");




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
                    //UserId = netUsers.UserId,
                    FirstName = netUsers.FirstName,
                    LastName = netUsers.LastName,
                    //Role = netUsers.RoleId,
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

        // POST: NetUserView/Edit/5
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
                        //RoleId = objNetUserViewModel.Role,
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
                    //Role = netUsers.RoleId,
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
        // POST: NetUserView/Delete/5
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




