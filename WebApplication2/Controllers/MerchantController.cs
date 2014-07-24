using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class MerchantController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Merchant/
        public ActionResult Index()
        {
            return View(db.MerchantViewModels.ToList());
        }

        // GET: /Merchant/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MerchantViewModel merchantviewmodel = db.MerchantViewModels.Find(id);
            if (merchantviewmodel == null)
            {
                return HttpNotFound();
            }
            return View(merchantviewmodel);
        }

        // GET: /Merchant/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Merchant/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Batch")] MerchantViewModel merchantviewmodel)
        {
            if (ModelState.IsValid)
            {
                db.MerchantViewModels.Add(merchantviewmodel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(merchantviewmodel);
        }

        // GET: /Merchant/Edit/5
        public ActionResult Manage()
        {
            var id = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = new MerchantDetailsViewModel
                        {
                            Merchant = db.MerchantViewModels.FirstOrDefault(f => f.UserId == id),
                            Vouchers = GetVouchersForMerchant(User.Identity.GetUserId())
                        };
            /*MerchantViewModel merchantviewmodel = */
           
            return View(model);
        }

        private List<BuyVoucherViewModel> GetVouchersForMerchant(string userId)
        {
            var gUserId = Guid.Parse(userId);
            return db.BuyVoucherViewModels.Where(d => d.MerchantSpentAtId.Equals(gUserId) && !d.CashedOutToMerchant && d.Spent).ToList();
        }

        public class MerchantDetailsViewModel
        {
            public List<BuyVoucherViewModel> Vouchers { get; set; }
            public MerchantViewModel Merchant { get; set; }
        }

        // POST: /Merchant/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage([Bind(Include = "Merchant,Vouchers")] MerchantDetailsViewModel merchantviewmodel)
        {
            merchantviewmodel.Vouchers = GetVouchersForMerchant(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                var merchant = db.MerchantViewModels.FirstOrDefault(i => i.UserId.Equals(merchantviewmodel.Merchant.UserId));
                try { merchant.ImageUrl = merchantviewmodel.Merchant.ImageUrl; }
                catch (Exception e) { }
                try { merchant.WebsiteUrl = merchantviewmodel.Merchant.WebsiteUrl; }
                catch (Exception e) { }
                try { merchant.WebsiteName = merchantviewmodel.Merchant.WebsiteName; }
                catch (Exception e) { }
                try { merchant.WebsiteDescription = merchantviewmodel.Merchant.WebsiteDescription; }
                catch (Exception e) { }
                try { merchant.VendorSelect = merchantviewmodel.Merchant.VendorSelect; }
                catch (Exception e) { }
                try { merchant.VendorDetails = merchantviewmodel.Merchant.VendorDetails; }
                catch (Exception e) { }
                try { merchant.EmailAddress = merchantviewmodel.Merchant.EmailAddress; }
                catch (Exception e) { }
                try
                {
                    merchant.Approved = false;
                    db.SaveChanges();
                    merchantviewmodel.Merchant = merchant;
                    return View(merchantviewmodel);
                }
                catch (Exception e) { return RedirectToAction("Index"); }
            }

            return View(merchantviewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestCashOut(MerchantPost state)
        {
            try
            {
                var user = db.MerchantViewModels.FirstOrDefault(a => a.UserId.Equals(state.UserId));
                user.PayoutRequested = state.CashoutRequested;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Json(new { success = false, message = e.Message, exception = e.InnerException });
            }
            return Json(new { success = true });
        }

        public class MerchantPost
        {
            public Boolean CashoutRequested { get; set; }
            public string UserId { get; set; }
        }

        // GET: /Merchant/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MerchantViewModel merchantviewmodel = db.MerchantViewModels.Find(id);
            if (merchantviewmodel == null)
            {
                return HttpNotFound();
            }
            return View(merchantviewmodel);
        }

        // POST: /Merchant/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MerchantViewModel merchantviewmodel = db.MerchantViewModels.Find(id);
            db.MerchantViewModels.Remove(merchantviewmodel);
            db.SaveChanges();
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
