using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Sipl.App_Start;
using Sipl.Areas.Admin.Models;
using Sipl.DataBase;
using Sipl.Models;

namespace Sipl.Areas.Admin.Controllers
{
    [Authorize]
    public class TeacherInfoController : Controller
    {
        SiplDatabaseEntities objEntities = new SiplDatabaseEntities();
        // GET: Admin/TeacherInfo
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult _TeacherGridPartial()
        {

            return View();
        }

        /// <summary>
        /// GET data from db
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UserSearchView()
        {
            try
            {
                //TO GET ROLES FROM DATABASE
                var roles = (from b in objEntities.NetRoles select b).ToList();
                var model = new FilterViewModel();

                model.Role = roles.Select(x => new SelectListItem
                {
                    Value = x.RoleId,
                    Text = x.RoleName
                }).ToList();

                //GET : COURSE FOR USERS
                var course = (from b in objEntities.Courses select b).ToList();
                model.Course = course.Select(x => new SelectListItem
                {
                    Value = x.CourseId.ToString(),
                    Text = x.CourseName
                });

                //TO GET COUNTRY ,STATES AND CITY
                var country = objEntities.Countries.ToList();
                List<SelectListItem> listCountry = new List<SelectListItem>();
                List<SelectListItem> listState = new List<SelectListItem>();
                List<SelectListItem> listCity = new List<SelectListItem>();

                listCountry.Add(new SelectListItem { Text = "", Value = "0" });
                listState.Add(new SelectListItem { Text = "", Value = "0" });
                listCity.Add(new SelectListItem { Text = "", Value = "0" });

                foreach (var m in country)
                {
                    listCountry.Add(new SelectListItem { Text = m.CountryName, Value = m.CountryId.ToString() });
                }
                ViewBag.country = listCountry;
                ViewBag.State = listState;
                ViewBag.City = listCity;

                List<SearchViewModel> objSearchViewModel = new List<SearchViewModel>();

                //Get List of users from DB
                var data = (from p in objEntities.NetUsers select p).ToList();
                foreach (var item in data)
                {
                    //To get Address From DB
                    var userAddressInfo = (from p in objEntities.Address
                                           where p.UserId == item.UserId
                                           select p).FirstOrDefault();

                    var getCourse = (from c in objEntities.Courses
                                     where c.CourseId == item.CourseId
                                     select c).FirstOrDefault();

                    var getRole = (from role in objEntities.NetRoles
                                   join user in objEntities.UserRole
                                   on role.RoleId equals user.RoleId
                                   where user.UserId == item.UserId
                                   select role.RoleName).FirstOrDefault();


                    if (userAddressInfo != null)

                    {
                        SearchViewModel searchView = new SearchViewModel
                        {
                            UserId = item.UserId,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            Gender = item.Gender,
                            Email = item.Email,
                            CourseId = item.CourseId,
                            RoleName = getRole,
                            DOB = item.DOB,
                            IsVerified = item.IsVerified,
                            IsActive = item.IsActive,


                            CurrentAddress = userAddressInfo.CurrentAddress,
                            PermanantAddress = userAddressInfo.PermanantAddress,
                            Country = userAddressInfo.Countries.CountryName,
                            States = userAddressInfo.States.StateName,
                            Cities = userAddressInfo.Cities.CityName,
                            Pincode = userAddressInfo.Pincode,
                            DateCreated = item.DateCreated
                        };
                        objSearchViewModel.Add(searchView);
                    }
                    model.List = objSearchViewModel;
                };


                return View(model);
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objFilterViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserSearchView(FilterViewModel objFilterViewModel)
        {

            //TO GET ROLES FROM DATABASE
            var roles = (from b in objEntities.NetRoles select b).ToList();
            var model = new FilterViewModel
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
            var country = objEntities.Countries.ToList();
            List<SelectListItem> listCountry = new List<SelectListItem>();
            List<SelectListItem> listState = new List<SelectListItem>();
            List<SelectListItem> listCity = new List<SelectListItem>();

            listCountry.Add(new SelectListItem { Text = "", Value = "0" });
            listState.Add(new SelectListItem { Text = "", Value = "0" });
            listCity.Add(new SelectListItem { Text = "", Value = "0" });

            foreach (var m in country)
            {
                listCountry.Add(new SelectListItem { Text = m.CountryName, Value = m.CountryId.ToString() });
            }
            ViewBag.country = listCountry;
            ViewBag.State = listState;
            ViewBag.City = listCity;



            //to compare filters' data in database.
            var searchBar = (from
                                  user in objEntities.NetUsers
                             join userRole in objEntities.UserRole on user.UserId equals userRole.UserId
                             join Address in objEntities.Address on user.UserId equals Address.UserId



                             where user.Courses.CourseId == objFilterViewModel.CourseId || objFilterViewModel.CourseId == null

                             where userRole.RoleId == objFilterViewModel.RoleId || string.IsNullOrEmpty(objFilterViewModel.RoleId)

                             select new SearchViewModel
                             {
                                 UserId = user.UserId,
                                 FirstName = user.FirstName,
                                 LastName = user.LastName,
                                 Gender = user.Gender,
                                 DOB = user.DOB,
                                 Email = user.Email,
                                 //IsEmailVerified = user.IsEmailVerified,
                                 IsActive = user.IsActive,
                                 CourseName = user.Courses.CourseName,
                                 CurrentAddress = Address.CurrentAddress,
                                 PermanantAddress = Address.PermanantAddress,
                                 Country = Address.Countries.CountryName,
                                 States = Address.States.StateName,
                                 Cities = Address.Cities.CityName,
                                 //Pincode = user.Address.Pincode,
                                 DateCreated = user.DateCreated,
                                 //DateModified = user.DateModified,
                                 RoleName = userRole.NetRoles.RoleName
                             }).ToList();

            model.List = searchBar;
            //return PartialView(model);
            return View(model);
        }

        /// <summary>
        /// Assin Subject
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult SubjectAssign(int id)
        {

            var subject = (from b in objEntities.Subjects select b).ToList();
            var model = new FilterViewModel
            {
                SubjectAssign = subject.Select(x => new SelectListItem
                {
                    Value = x.SubjectId.ToString(),
                    Text = x.SubjectName
                }).ToList()
            };

            var getUser = (from d in objEntities.Address
                           join c in objEntities.NetUsers on d.UserId equals c.UserId
                           join s in objEntities.UserRole on c.UserId equals s.UserId
                           where d.UserId == id
                           select new FilterViewModel
                           {
                               UserId = c.UserId,
                               FirstName = c.FirstName
                           }).FirstOrDefault();
            model.FirstName = getUser.FirstName;

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubjectAssign(int id, FilterViewModel objFilterViewModel)
        {
            {
                ViewBag.Subject = new SelectList(objEntities.Subjects.ToList(), "SubjectId", "SubjectName");
                try
                {
                    if (ModelState.IsValid)
                    {
                        var getUser = (from d in objEntities.Address
                                       join c in objEntities.NetUsers on d.UserId equals c.UserId
                                       join s in objEntities.UserRole on c.UserId equals s.UserId
                                       where d.UserId == id
                                       select new FilterViewModel
                                       {
                                           UserId = c.UserId,
                                           FirstName = c.FirstName
                                       }).FirstOrDefault();

                        TeacherInSubject objTeacherInSubject = new TeacherInSubject
                        {

                            UserId = getUser.UserId,
                            SubjectId = objFilterViewModel.SubjectId,

                        };
                        objEntities.TeacherInSubject.Add(objTeacherInSubject);
                        objEntities.SaveChanges();
                        ModelState.Clear();
                        return RedirectToAction("TeacherInSubjectlist", "TeacherInfo");
                    }
                    return View(objFilterViewModel);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception source: {0} user is failed to register", ex.Message);
                    return View(ex);
                }
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
        public JsonResult GetCity(int id)
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

        /// <summary>
        /// list for subjectAssign
        /// </summary>
        public ActionResult TeacherInSubjectlist()
        {

            var getUser = (from d in objEntities.Address
                           join c in objEntities.NetUsers on d.UserId equals c.UserId
                           join s in objEntities.TeacherInSubject on c.UserId equals s.UserId
                           select new FilterViewModel
                           {
                               UserId = c.UserId,
                               FirstName = c.FirstName,
                               SubjectId = s.SubjectId,
                               Subjects = s.Subjects
                           }).ToList();


            return View(getUser);
        }

        /// <summary>
        /// list of students for teacher
        /// </summary>
        /// <returns></returns>
        public ActionResult _TeacherPage(int? id)
        {
            List<NetUserViewModel> results = new List<NetUserViewModel>();
            if (Request.IsAuthenticated)
            {
                //teacher list
                ViewBag.SendId = id;
                TempData["TeacherId"] = id;
                var innerQuery = (from user in objEntities.NetUsers
                                  join teacherInSubject in objEntities.TeacherInSubject on user.UserId equals teacherInSubject.UserId
                                  join subjectInCourse in objEntities.SubjectInCourse on teacherInSubject.SubjectId equals subjectInCourse.SubjectId
                                  where user.UserId == id
                                  select subjectInCourse.CourseId
                            ).ToList();
                if (innerQuery.Count > 0)
                {
                    foreach (var item in innerQuery)
                    {
                        List<NetUserViewModel> result = new List<NetUserViewModel>();
                        result = (from user in objEntities.NetUsers
                                  where user.CourseId == item
                                  select new NetUserViewModel
                                  {
                                      UserId = user.UserId,
                                      FirstName = user.FirstName,
                                      LastName = user.LastName,
                                      CourseName = user.Courses.CourseName,
                                  }
                                       ).ToList();

                        results.AddRange(result);
                    }
                }
                return View(results);
            }
            else
            {
                return RedirectToAction("LogIn");
            }
        }

        /// <summary>
        /// get method for student list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult UserProfile(int? id)
        {
            if (Request.IsAuthenticated)
            {
                //var user = User.Identity.Name;

                var userList = (from d in objEntities.NetUsers
                                join c in objEntities.UserRole on d.UserId equals c.UserId
                                join s in objEntities.Address on c.UserId equals s.UserId
                                join e in objEntities.SubjectInCourse on d.CourseId equals e.CourseId
                                where d.UserId == id
                                select new NetUserViewModel
                                {
                                    UserId = d.UserId,
                                    RoleId = c.RoleId,
                                    CourseName = d.Courses.CourseName,
                                    FirstName = d.FirstName,
                                    LastName = d.LastName,
                                    Gender = d.Gender,
                                    Email = d.Email,
                                    Password = d.Password,
                                    DOB = d.DOB,
                                    IsActive = d.IsActive,
                                    IsVerified = d.IsVerified,
                                    CurrentAddress = s.CurrentAddress,
                                    PermanantAddress = s.PermanantAddress,
                                    Country = s.Countries.CountryName,
                                    States = s.States.StateName,
                                    Cities = s.Cities.CityName,
                                    DateCreated = d.DateCreated,
                                    Pincode = s.Pincode
                                }).FirstOrDefault();
                return View(userList);
            }
            else
            {
                return RedirectToAction("LogIn");
            }
        }

        /// <summary>
        /// Method for teacher HomePage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult TeacherProfile(int? id)
        {
            if (Request.IsAuthenticated)
            {
                //var user = User.Identity.Name;
                Session["Id"] = id;

                var userList = (from d in objEntities.NetUsers
                                join c in objEntities.UserRole on d.UserId equals c.UserId
                                join s in objEntities.Address on c.UserId equals s.UserId
                                join f in objEntities.TeacherInSubject on c.UserId equals f.UserId
                                join su in objEntities.Subjects on f.SubjectId equals su.SubjectId
                                where d.UserId == id
                                select new NetUserViewModel
                                {
                                    UserId = d.UserId,
                                    RoleId = c.RoleId,
                                    SubjectName = su.SubjectName,
                                    FirstName = d.FirstName,
                                    LastName = d.LastName,
                                    Gender = d.Gender,
                                    Email = d.Email,
                                    Password = d.Password,
                                    DOB = d.DOB,
                                    IsActive = d.IsActive,
                                    IsVerified = d.IsVerified,
                                    CurrentAddress = s.CurrentAddress,
                                    PermanantAddress = s.PermanantAddress,
                                    Country = s.Countries.CountryName,
                                    States = s.States.StateName,
                                    Cities = s.Cities.CityName,
                                    DateCreated = d.DateCreated,
                                    Pincode = s.Pincode
                                }).FirstOrDefault();

                if (userList == null)
                {
                    var userListnull = (from d in objEntities.NetUsers
                                        join c in objEntities.UserRole on d.UserId equals c.UserId
                                        join s in objEntities.Address on c.UserId equals s.UserId
                                        where d.UserId == id
                                        select new NetUserViewModel
                                        {
                                            UserId = d.UserId,
                                            RoleId = c.RoleId,
                                            //SubjectName = su.SubjectName,
                                            FirstName = d.FirstName,
                                            LastName = d.LastName,
                                            Gender = d.Gender,
                                            Email = d.Email,
                                            Password = d.Password,
                                            DOB = d.DOB,
                                            IsActive = d.IsActive,
                                            IsVerified = d.IsVerified,
                                            CurrentAddress = s.CurrentAddress,
                                            PermanantAddress = s.PermanantAddress,
                                            Country = s.Countries.CountryName,
                                            States = s.States.StateName,
                                            Cities = s.Cities.CityName,
                                            DateCreated = d.DateCreated,
                                            Pincode = s.Pincode
                                        }).FirstOrDefault();
                    return View(userListnull);


                }
                else {

                    return View(userList);
                }
            }
            else
            {
                return RedirectToAction("LogIn");
            }
        }

        /// <summary>
        /// Student page where Action links 
        /// </summary>
        /// <returns></returns>
        public ActionResult _StudentPage(int? id)
        {
            ViewBag.UserId = id;
            var courseDetails =
                    (from
                     user in objEntities.NetUsers
                     join subjectInCourse in objEntities.SubjectInCourse on user.CourseId equals subjectInCourse.CourseId
                     join subject in objEntities.Subjects on subjectInCourse.SubjectId equals subject.SubjectId
                     join teacherInSubject in objEntities.TeacherInSubject on subjectInCourse.SubjectId equals teacherInSubject.SubjectId
                     where user.UserId == id
                     select new NetUserViewModel
                     {
                         UserId = user.UserId,
                         SubjectId = teacherInSubject.SubjectId,
                         SubjectName = subject.SubjectName,
                         FirstName = teacherInSubject.NetUsers.FirstName,
                         LastName = teacherInSubject.NetUsers.LastName,
                         CourseId = user.CourseId,
                         CourseName = user.Courses.CourseName
                     }).ToList();
            return View(courseDetails);
        }
    }
}



