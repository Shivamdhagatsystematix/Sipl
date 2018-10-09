using Sipl.DataBase;
using Sipl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Sipl.Controllers
{
    public class LogInController : Controller
    {


        // GET: LogIn/Create
        [AllowAnonymous]
        public ActionResult LogIn()
        {
            return View();
        }
       
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(LoginViewModel model, string returnUrl)
        {
            try
            {
                {
                    if (!ModelState.IsValid)
                    {
                        return View(model);
                    }
                    using (var SiplDatabaseEntities = new SiplDatabaseEntities())
                    {
                        var result = SiplDatabaseEntities.NetUsers.Any(x => x.Email == model.Email && x.Password == model.Password);
                        switch (result)
                        {
                            case true:
                                FormsAuthentication.SetAuthCookie(model.Email, true);
                                resetRequest();
                                return RedirectToAction("LoggedIn");
                            default:
                                ModelState.AddModelError("", "Invalid login attempt.");
                                return View(model);
                        }
                    }
                }
                //if (model.Email != null && model.Password != null)
                //{
                //    using (SiplDatabaseEntities objSiplDatabaseEntities = new SiplDatabaseEntities())
                //    {
                //        var obj = objSiplDatabaseEntities.NetUsers.Where(u => u.Email.Equals
                //        (model.Email) && u.Password.Equals(model.Password)).FirstOrDefault();
                //        if (obj != null)
                //        {
                //            Session["Email"] = obj.Email.ToString();
                //            Session["Password"] = obj.Password.ToString();
                //            return RedirectToAction("LoggedIn");
                //        }
                //        else
                //        {
                //            Session["Email"] = null;
                //            Session["Password"] = null;

                //            return View(model);
                //        }
                //    }
                //}
                //else { return View(model); }



            }
            catch
            {
                return View();
            }
        }
        public ActionResult LoggedIn()
        {
            if(Request.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("LogIn");
            }
        }


        // GET:  WELCOME SCREEN
        public ActionResult RegisteredUser()
        {
            return View();
        }


        // POST: Welcome Screen
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
            //return RedirectToAction("Index", "Home");
            //Session["Email"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
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
