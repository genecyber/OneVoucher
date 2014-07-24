using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        //
        // GET: /Admin/
        public ActionResult Index()
        {
            var admin = new AdminViewModel
                        {
                            Vouchers = db.BuyVoucherViewModels.ToList(),
                            Merchants = db.MerchantViewModels.ToList()
                        };
            return View(admin);
        }

        public class AdminViewModel
        {
            public List<BuyVoucherViewModel> Vouchers { get; set; }
            public List<MerchantViewModel> Merchants { get; set; }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateMerchantApproval(MerchantApprovalPost state)
        {
            try
            {
                var user = db.MerchantViewModels.FirstOrDefault(a => a.UserId.Equals(state.UserId));
                user.Approved = state.Approved;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Json(new { success = false, message = e.Message, exception = e.InnerException }); 
            }
            return Json(new { success = true });
        }

        public class MerchantApprovalPost
        {
            public Boolean Approved { get; set; }
            public string UserId { get; set; }
        }

        //
        // GET: /Admin/Details/5
        public ActionResult AdminVoucherDetails(Guid id)
        {
            return View(db.BuyVoucherViewModels.FirstOrDefault(d => d.Id.Equals(id)));
        }

        //
        // GET: /Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VendorSelect,AmoutToLoad,Paid,Spent,LoadTxId, EmailAddress")] BuyVoucherViewModel buyvoucherviewmodel)
        {
            if (ModelState.IsValid)
            {
                buyvoucherviewmodel.VoucherGuid = Guid.NewGuid();
                buyvoucherviewmodel.Id = Guid.NewGuid();
                db.BuyVoucherViewModels.Add(buyvoucherviewmodel);
                db.SaveChanges();
                TempData["pay"] = buyvoucherviewmodel;

                return RedirectToAction("Index");// View(buyvoucherviewmodel);
            }

            return View();
        }

        //
        // GET: /Admin/Edit/5
        public ActionResult Edit(Guid id)
        {
            return View(db.BuyVoucherViewModels.FirstOrDefault(d => d.Id.Equals(id)));
        }

        //
        // POST: /Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(Guid id, BuyVoucherViewModel buyvoucherviewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(buyvoucherviewmodel).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(db.BuyVoucherViewModels.FirstOrDefault(d => d.Id.Equals(id)));
            }
        }

        //
        // GET: /Admin/Payout/5
        public ActionResult Payout(Guid id)
        {
            return View(db.BuyVoucherViewModels.FirstOrDefault(d => d.Id.Equals(id)));
        }

        //
        // POST: /Admin/Payout/5
        [HttpPost]
        public ActionResult Payout(Guid id, BuyVoucherViewModel buyvoucherviewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(buyvoucherviewmodel).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(db.BuyVoucherViewModels.FirstOrDefault(d => d.Id.Equals(id)));
            }
        }

        //
        // GET: /Admin/Delete/5
        public ActionResult Delete(Guid id)
        {
            var toDelete = db.BuyVoucherViewModels.FirstOrDefault(d => d.Id.Equals(id));
            return View(toDelete);
        }

        //
        // POST: /Admin/Delete/5
        [HttpPost]
        public ActionResult Delete(Guid id, FormCollection collection)
        {
            try
            {
                var toDelete = db.BuyVoucherViewModels.FirstOrDefault(d => d.Id.Equals(id));
                db.BuyVoucherViewModels.Remove(toDelete);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
