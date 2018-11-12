using Sipl.App_Start;
using Sipl.DataBase;
using Sipl.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Sipl.Controllers
{
    public class LogInController : Controller
    {
        SiplDatabaseEntities objEntities = new SiplDatabaseEntities();
        /// <summary>
        ///  Get :Method for LogIn by Users
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult LogIn()
        {
            var model = new LoginViewModel();
            ViewBag.Title = "Login";
            model.Email = CheckLoginCookie();
            model.RememberMe = !string.IsNullOrEmpty(model.Email);
            return View("Login", model);
        }

        /// <summary>
        /// To check login Cookies
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        private string CheckLoginCookie()
        {
            if (Request.Cookies.Get("Email") != null)
            {
                return Request.Cookies["Email"].Value;
            }
            return string.Empty;
        }

        /// <summary>
        ///POST : Method for LogIn
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(LoginViewModel model, string returnUrl)
        {
            {
                //do lots of stuff
                //Create username remember cookie
                if (model.RememberMe)
                {
                    HttpCookie ckEmail = new HttpCookie("Email");
                    ckEmail.Expires = DateTime.Now.AddSeconds(500);
                    ckEmail.Value = model.Email;
                    Response.Cookies.Add(ckEmail);
                }
            }
            NetUsers netuser = new NetUsers();
            try
            {
                if (model.Email != null && model.Password != null && model.RememberMe != null)
                {
                    using (SiplDatabaseEntities objSiplDatabaseEntities = new SiplDatabaseEntities())
                    {
                        //To Encode Password enter by User to match it with Database
                        var keyNew = "Test";
                        var password = Helper.EncodePassword(model.Password, keyNew);
                        //To check Email & Password From DB
                        var obj = objSiplDatabaseEntities.NetUsers.Where
                         (u => u.Email == model.Email && u.Password == password)
                         .FirstOrDefault();

                        if (obj != null)
                        {
                            FormsAuthentication.SetAuthCookie(model.Email, true);
                            var isAdmin = (from role in objSiplDatabaseEntities.NetRoles
                                           join user in objSiplDatabaseEntities.UserRole
                                           on role.RoleId equals user.RoleId
                                           where user.UserId == obj.UserId
                                           select role.RoleName).FirstOrDefault();


                            if (isAdmin == "Admin")
                            {
                                Session["RoleId"] = 3;
                                Session["RoleName"] = "Admin";
                                return RedirectToAction("UserSearchView", "Admin/TeacherInfo");
                            }
                            else if (isAdmin == "Teacher")
                            {
                                Session["RoleId"] =1;
                                Session["RoleName"] = "Teacher";
                                return RedirectToAction("TeacherProfile", "Admin/TeacherInfo", new { id = obj.UserId });


                            }
                            else if (isAdmin == "Student")
                            {
                                Session["RoleId"] = 2;
                                Session["RoleName"] = "Student";
                                return RedirectToAction("UserProfile", "Admin/TeacherInfo", new { id = obj.UserId });

                            }
                            else
                            {
                                Session["Email"] = null;
                                Session["Password"] = null;
                                return View(model);

                            }
                        }
                        else
                        {
                            ModelState.AddModelError("Email", "Email and Password not found or matched");
                            return View(model);
                        }

                    }
                }

                else return View(model);
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0} Login Failed", ex.Message);
                return View();
            }
        }

        /// <summary>
        /// Admin page
        /// </summary>
        /// <returns></returns>
        public ActionResult _AdminPage()
        {
            if (Request.IsAuthenticated)
            {
                var userList = (from d in objEntities.NetUsers
                                join c in objEntities.UserRole on d.UserId equals c.UserId
                                join s in objEntities.Address on c.UserId equals s.UserId
                                where c.RoleId != "3"

                                select new NetUserViewModel
                                {
                                    UserId = d.UserId,
                                    RoleId = c.RoleId,
                                    CourseId = Convert.ToInt16(d.Courses.CourseName),
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
                                    DateCreated = d.DateCreated
                                }).ToList();
                return View(userList);
            }
            else
            {
                return RedirectToAction("LogIn");
            }
        }

        /// <summary>
        ///  LoggedIn method
        /// </summary>
        /// <returns></returns>
        public ActionResult LoggedIn()
        {
            if (Request.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("LogIn");
            }
        }

        /// <summary>
        /// Registered User Method for Welcome Page after Successfull Registration
        /// </summary>
        /// <returns></returns>
        public ActionResult RegisteredUser()
        {
            return View();
        }

        /// <summary>
        ///Successfull Registraion 
        /// </summary>
        /// <param name="objNetUsers"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisteredUser(NetUsers objNetUsers)
        {
            try
            {
                {
                    if (Session["FirstName"] != null)
                    {
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("LogIn");
                    }
                };
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// : LogOutMethod for Clearing sessions
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOut()
        {
            Response.AddHeader("Cache-Control", "no-cache, no-store,must-revalidate");
            Response.AddHeader("Pragma", "no-cache");
            Response.AddHeader("Expires", "0");
            Session.Abandon();
            Session.Clear();
            Response.Cookies.Clear();
            Session.RemoveAll();
            Session["Login"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        ///  Redirect Method For Unsuccessfull LogIn Attempts
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Method for Form Authentication
        /// </summary>
        private void resetRequest()
        {
            var authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null && !authTicket.Expired)
                {
                    var roles = authTicket.UserData.Split(',');
                    System.Web.HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new FormsIdentity(authTicket), roles);
                }
            }
        }
    }
}




