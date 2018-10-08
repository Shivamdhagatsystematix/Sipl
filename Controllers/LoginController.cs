using Sipl.DataBase;
using Sipl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sipl.Controllers
{
    public class LogInController : Controller
    {
    

        // GET: LogIn/Create
        public ActionResult LogIn()
        {
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(NetUsers objNetUsers)
        {
            try
            {
                if (objNetUsers.Email != null && objNetUsers.Password != null)
                {
                    using (SiplDatabaseEntities objSiplDatabaseEntities = new SiplDatabaseEntities())
                    {
                        var obj = objSiplDatabaseEntities.NetUsers.Where(u => u.Email.Equals
                        (objNetUsers.Email) && u.Password.Equals(objNetUsers.Password)).FirstOrDefault();
                        if (obj != null)
                        {
                            Session["Email"] = obj.Email.ToString();
                            Session["Password"] = obj.Password.ToString();
                            return RedirectToAction("LoggedIn");
                        }
                    }
                }

                        return View(objNetUsers);


        }
            catch
            {
                return View();
            }
        }
        public ActionResult LoggedIn()
        {
            if (Session["Email"] != null)
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

        //POST:
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

        // POST: LogIn/Create



    }
}
