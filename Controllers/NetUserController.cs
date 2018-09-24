using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sipl.Models;

namespace Sipl.Controllers
{
    public class NetUserController : Controller
    {
        private SiplDatabaseEntities10 db = new SiplDatabaseEntities10();

        // GET: NetUser
        public async Task<ActionResult> Index()
        {
            return View(await db.NetUsers.ToListAsync());
        }

        // GET: NetUser/Details/5
        public async Task<ActionResult> Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NetUser netUser = await db.NetUsers.FindAsync(id);
            if (netUser == null)
            {
                return HttpNotFound();
            }
            return View(netUser);
        }

        // GET: NetUser/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NetUser/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "UserId,FirstName,LastName,Gender,Email,Password,ConfirmPassword,AddressId,D_O_B_,IsActive,DateCreated,DateModified")] NetUser netUser)
        {
            if (ModelState.IsValid)
            {
                db.NetUsers.Add(netUser);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(netUser);
        }

        // GET: NetUser/Edit/5
        public async Task<ActionResult> Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NetUser netUser = await db.NetUsers.FindAsync(id);
            if (netUser == null)
            {
                return HttpNotFound();
            }
            return View(netUser);
        }

        // POST: NetUser/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UserId,FirstName,LastName,Gender,Email,Password,ConfirmPassword,AddressId,D_O_B_,IsActive,DateCreated,DateModified")] NetUser netUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(netUser).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(netUser);
        }

        // GET: NetUser/Delete/5
        public async Task<ActionResult> Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NetUser netUser = await db.NetUsers.FindAsync(id);
            if (netUser == null)
            {
                return HttpNotFound();
            }
            return View(netUser);
        }

        // POST: NetUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(byte id)
        {
            NetUser netUser = await db.NetUsers.FindAsync(id);
            db.NetUsers.Remove(netUser);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
