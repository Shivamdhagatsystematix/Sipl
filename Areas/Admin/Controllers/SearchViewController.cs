using Sipl.Areas.Admin.Models;
using Sipl.DataBase;
using Sipl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sipl.Areas.Admin.Controllers
{
    public class SearchViewController : Controller
    {
        SiplDatabaseEntities objEntities = new SiplDatabaseEntities();

        public ActionResult _SearchGridPartial()
        {
            Test test = new Test();
            // Get user roles from DB
            var roles = (from b in objEntities.NetRoles select b).ToList();
            var model = new SearchViewModel
            {
                Role = roles.Select(x => new SelectListItem
                {
                    Value = x.RoleId,
                    Text = x.RoleName
                }).ToList()
            };
            var course = (from b in objEntities.Courses select b).ToList();
            model.Course = course.Select(x => new SelectListItem
            {
                Value = x.CourseId.ToString(),
                Text = x.CourseName
            });

            ViewBag.Role = new SelectList(objEntities.NetRoles.ToList(), "RoleId", "RoleName");
            List<SearchViewModel> objSearchViewModel = new List<SearchViewModel>();

            //Get List of users from DB
            var data = (from p in objEntities.NetUsers select p).ToList();
            foreach (var item in data)
            {
                //To get Address From DB
                var userAddressInfo = (from p in objEntities.Address where p.UserId == item.UserId select p).FirstOrDefault();
                if (userAddressInfo != null)
                {
                    SearchViewModel searchView = new SearchViewModel
                    {
                      
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Gender = item.Gender,
                        Email = item.Email,
                      
                        DOB = item.DOB,
                        IsActive = item.IsActive,
                        CurrentAddress = userAddressInfo.CurrentAddress,
                        PermanantAddress = userAddressInfo.PermanantAddress,
                        Country = userAddressInfo.Countries.CountryName,
                        States = userAddressInfo.States.StateName,
                        Cities = userAddressInfo.Cities.CityName,
                        DateCreated = item.DateCreated
                    };
                    objSearchViewModel.Add(searchView);
                }
            };
            test.List = objSearchViewModel;
            return PartialView(test);
        }
        /// <summary>
        /// GET data from db
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchView()
         {
            Test test = new Test();
            //TO GET ROLES FROM DATABASE
            var roles = (from b in objEntities.NetRoles select b).ToList();
            var model = new Test
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
            }



            List<SearchViewModel> objSearchViewModel = new List<SearchViewModel>();

            //Get List of users from DB
            var data = (from p in objEntities.NetUsers select p).ToList();
            foreach (var item in data)
            {
                //To get Address From DB
                var userAddressInfo = (from p in objEntities.Address where p.UserId == item.UserId select p).FirstOrDefault();
                if (userAddressInfo != null)
                {
                    SearchViewModel searchView = new SearchViewModel
                    {

                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Gender = item.Gender,
                        Email = item.Email,

                        DOB = item.DOB,
                        IsActive = item.IsActive,
                        CurrentAddress = userAddressInfo.CurrentAddress,
                        PermanantAddress = userAddressInfo.PermanantAddress,
                        Country = userAddressInfo.Countries.CountryName,
                        States = userAddressInfo.States.StateName,
                        Cities = userAddressInfo.Cities.CityName,
                        DateCreated = item.DateCreated
                    };
                    objSearchViewModel.Add(searchView);
                }
            };
            //test.List = objSearchViewModel;
            model.List = objSearchViewModel;

            return View(model);
        }

    }
}
