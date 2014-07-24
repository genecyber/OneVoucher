using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Helpers;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class BuyVoucherController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /BuyVoucher/
        public ActionResult Index()
        {
            //return View();
            return RedirectToAction("Buy");
        }

        // GET: /BuyVoucher/Check
        public ActionResult Check()
        {
            return View();
        }

        // POST: /BuyVoucher/Check
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RateLimit.RateLimitAttribute(Seconds = 5)]
        public ActionResult Check(string VoucherGuid)
        {
            Guid voucherGuid;
            if (Guid.TryParse(VoucherGuid, out voucherGuid))
            {
                var buyvoucherviewmodel = db.BuyVoucherViewModels.FirstOrDefault(d => d.VoucherGuid == voucherGuid);
                if (buyvoucherviewmodel == null)
                {
                    return View(new BuyVoucherViewModel());
                }
                return View(buyvoucherviewmodel);
            }
            return View(new BuyVoucherViewModel());
        }

        // POST: /BuyVoucher/Spend
        [HttpPost]
        public ActionResult Spend(SpendRequest spend)
        {

            //decrypt and verify spend
            //if good, update voucher and add spend record
            //encrypt response with secret
            var response = new SpendResponse();
            return Json(spend, JsonRequestBehavior.DenyGet); 
        }

        // GET: /BuyVoucher/Buy
        public ActionResult Buy()
        {
            return View();
        }

        // POST: /BuyVoucher/Buy
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Buy([Bind(Include = "VendorSelect,AmoutToLoad,EmailAddress")] BuyVoucherViewModel buyvoucherviewmodel)
        {
            if (ModelState.IsValid)
            {
                buyvoucherviewmodel.VoucherGuid = Guid.NewGuid();
                buyvoucherviewmodel.Id = Guid.NewGuid();
                db.BuyVoucherViewModels.Add(buyvoucherviewmodel);
                db.SaveChanges();
                TempData["pay"] =  buyvoucherviewmodel;

                return RedirectToAction("Pay");// View(buyvoucherviewmodel);
            }

            return View();
        }

        // GET: /BuyVoucher/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuyVoucherViewModel buyvoucherviewmodel = db.BuyVoucherViewModels.Find(id);
            if (buyvoucherviewmodel == null)
            {
                return HttpNotFound();
            }
            return View(buyvoucherviewmodel);
        }

        // POST: /BuyVoucher/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Vendor,Amount")] BuyVoucherViewModel buyvoucherviewmodel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(buyvoucherviewmodel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(buyvoucherviewmodel);
        }

        // GET: /BuyVoucher/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuyVoucherViewModel buyvoucherviewmodel = db.BuyVoucherViewModels.Find(id);
            if (buyvoucherviewmodel == null)
            {
                return HttpNotFound();
            }
            return View(buyvoucherviewmodel);
        }

        // POST: /BuyVoucher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            BuyVoucherViewModel buyvoucherviewmodel = db.BuyVoucherViewModels.Find(id);
            db.BuyVoucherViewModels.Remove(buyvoucherviewmodel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: /BuyVoucher/Pay
        [ActionName("Pay")]
        public ActionResult Pay(BuyVoucherViewModel buyvoucherviewmodel)
        {
            var obj = TempData["pay"];
            buyvoucherviewmodel = obj as BuyVoucherViewModel;
            var model = buyvoucherviewmodel;
            return View(buyvoucherviewmodel);
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
