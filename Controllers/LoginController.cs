using Sipl.App_Start;
using Sipl.DataBase;
using Sipl.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace Sipl.Controllers
{
    public class LogInController : Controller
    {
        SiplDatabaseEntities objNetUserViewModel = new SiplDatabaseEntities();
        /// <summary>
        ///  Get :Method for LogIn by Users
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult LogIn()
        {
            return View();
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
            NetUsers netuser = new NetUsers();
            try
            {
                if (model.Email != null && model.Password != null)
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
                                Session["Admin"] = true;
                                Session["Teacher"] = false;
                                Session["Student"] = false;
                            }
                            else if (isAdmin == "teacher")
                            {
                                Session["Admin"] = false;
                                Session["Teacher"] = true;
                                Session["Student"] = false;
                            }
                            else
                            {
                                Session["Admin"] = false;
                                Session["Teacher"] = false;
                                Session["Student"] = true;

                            }
                            Session["Email"] = obj.Email.ToString();
                            Session["Password"] = obj.Password.ToString();
                            return RedirectToAction("LoggedIn");
                        }
                        else
                        {
                            Session["Email"] = null;
                            Session["Password"] = null;
                            return View(model);
                        }
                    }
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                return View();
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
