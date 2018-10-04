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
                var userAddressInfo = (from p in objEntities.Address select p).ToList().
                    FirstOrDefault(u => u.UserId == item.UserId);
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
                        ConfirmPassword = item.ConfirmPassword,
                        DOB = item.DOB,
                        IsActive = item.IsActive,
                        CurrentAddress = userAddressInfo.CurrentAddress,
                        PermanantAddress = userAddressInfo.PermanantAddress,
                        CountryId = userAddressInfo.CountryId,
                        StateId = userAddressInfo.StateId,
                        CityId = userAddressInfo.CityId,
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
                    ConfirmPassword = netUsers.ConfirmPassword,
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
            List<CountryModel> listCountryModel = new List<CountryModel>();
            List<StateModel> listStateModel = new List<StateModel>();
            List<CityModel> listCityModel = new List<CityModel>();

            var Roles = (from b in objEntities.NetRoles select b).ToList();

            var country = (from b in objEntities.Countries select b).ToList();
            var state = (from b in objEntities.States select b).ToList();
            var city = (from b in objEntities.Cities select b).ToList();

            var model = new NetUserViewModel
            {
                Role = Roles.Select(x => new SelectListItem
                {
                    Value = x.RoleId,
                    Text = x.RoleName
                })


            };

            foreach (var item in country)
            {
                CountryModel countryModel = new CountryModel
                {
                    CountryId = item.CountryId,
                    CountryName = item.CountryName
                };
                listCountryModel.Add(countryModel);

            }
            foreach (var item in state)
            {
                StateModel stateModel = new StateModel
                {
                  StateId = item.StateId,
                    StateName = item.StateName
                };
                listStateModel.Add(stateModel);

            }

            foreach (var item in city)
            {
               CityModel cityModel = new CityModel
                {
                   CityId = item.CityId,
                 CityName = item.CityName
               };
                listCityModel.Add(cityModel);

            }
            ViewBag.CountryData = listCountryModel;

            ViewBag.StateData = listStateModel;
            ViewBag.CityData = listCityModel;


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
                        Email = objNetUserViewModel.Email,
                        Password = objNetUserViewModel.Password,
                        ConfirmPassword = objNetUserViewModel.ConfirmPassword,
                        DOB = objNetUserViewModel.DOB,
                        IsActive = objNetUserViewModel.IsActive,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now
                    };


                    var test = objEntities.NetUsers.Add(objNetUsers);
                    objEntities.SaveChanges();
                    var userId = objNetUsers.UserId;


                    UserRole objUserRole = new UserRole
                    {
                        RoleId = objNetUserViewModel.RoleId,
                        UserId = userId
                    };
                    objEntities.UserRole.Add(objUserRole);


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
                    //Role = netUsers.RoleId,
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


    }
}

