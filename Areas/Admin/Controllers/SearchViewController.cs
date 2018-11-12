using Sipl.Areas.Admin.Models;
using Sipl.DataBase;
using Sipl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Sipl.Areas.Admin.Controllers
{
    [Authorize]
    public class SearchViewController : Controller

    {
        SiplDatabaseEntities objEntities = new SiplDatabaseEntities();
        /// <summary>
        /// Partial search view grid
        /// </summary>
        /// <returns></returns>
        public ActionResult _SearchGridPartial()
        {
            return View();
        }

        /// <summary>
        /// GET data from db
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SearchView()
        {
            try
            {
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
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            Gender = item.Gender,
                            Email = item.Email,
                            CourseId = item.CourseId,
                            RoleName = getRole,
                            DOB = item.DOB,
                            IsActive = item.IsActive,
                            CurrentAddress = userAddressInfo.CurrentAddress,
                            PermanantAddress = userAddressInfo.PermanantAddress,
                            Country = userAddressInfo.Countries.CountryName,
                            States = userAddressInfo.States.StateName,
                            Cities = userAddressInfo.Cities.CityName,
                            Pincode = userAddressInfo.Pincode,
                            IsVerified = item.IsVerified,
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
        /// Search Page 
        /// </summary>
        /// <param name="objFilterViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchView(FilterViewModel objFilterViewModel)
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
                             where user.FirstName == objFilterViewModel.FirstName || string.IsNullOrEmpty(objFilterViewModel.FirstName)
                             where user.LastName == objFilterViewModel.LastName || string.IsNullOrEmpty(objFilterViewModel.LastName)
                             where user.Gender == objFilterViewModel.Gender || string.IsNullOrEmpty(objFilterViewModel.Gender)
                             where user.Email == objFilterViewModel.Email || string.IsNullOrEmpty(objFilterViewModel.Email)
                             where user.DOB == objFilterViewModel.DOB || objFilterViewModel.DOB == null
                             where user.IsActive == objFilterViewModel.IsActive
                             where user.IsVerified == objFilterViewModel.IsVerified
                             where user.Courses.CourseId == objFilterViewModel.CourseId || objFilterViewModel.CourseId == null
                             where Address.CurrentAddress == objFilterViewModel.CurrentAddress || string.IsNullOrEmpty(objFilterViewModel.CurrentAddress)
                             where Address.PermanantAddress == objFilterViewModel.PermanantAddress || string.IsNullOrEmpty(objFilterViewModel.PermanantAddress)
                             where Address.Countries.CountryId == objFilterViewModel.CountryId || string.IsNullOrEmpty(objFilterViewModel.Country)
                             where Address.States.StateId == objFilterViewModel.StateId || string.IsNullOrEmpty(objFilterViewModel.States)
                             where Address.Cities.CityId == objFilterViewModel.CityId || string.IsNullOrEmpty(objFilterViewModel.Cities)
                             where Address.Pincode == model.Pincode.ToString() || model.Pincode == null
                             where userRole.RoleId == objFilterViewModel.RoleId || string.IsNullOrEmpty(objFilterViewModel.RoleId)
                             select new SearchViewModel
                             {
                                 FirstName = user.FirstName,
                                 LastName = user.LastName,
                                 Gender = user.Gender,
                                 DOB = user.DOB,
                                 Email = user.Email,
                                 IsVerified = user.IsVerified,
                                 IsActive = user.IsActive,
                                 CourseName = user.Courses.CourseName,
                                 CurrentAddress = Address.CurrentAddress,
                                 PermanantAddress = Address.PermanantAddress,
                                 Country = Address.Countries.CountryName,
                                 States = Address.States.StateName,
                                 Cities = Address.Cities.CityName,
                                 Pincode = Address.Pincode,
                                 DateCreated = user.DateCreated,
                                 RoleName = userRole.NetRoles.RoleName
                             }).ToList();
            model.List = searchBar;
            return View(model);
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
    }
}
