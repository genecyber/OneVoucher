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

        private List<VendorSpendModel> GetVouchersForMerchant(string userId)
        {
            return db.VendorSpendModels.Where(d => d.Merchant.UserId.Equals(userId) && !d.Voucher.MerchantPaid).ToList();
        }

        public class MerchantDetailsViewModel
        {
            public List<VendorSpendModel> Vouchers { get; set; }
            public MerchantViewModel Merchant { get; set; }
        }

        // POST: /Merchant/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage([Bind(Include = "UserId,ImageUrl,WebsiteUrl,WebsiteName,WebsiteDescription,VendorSelect,VendorDetails,EmailAddress")] MerchantViewModel merchantviewmodel)
        {
            if (ModelState.IsValid)
            {
                var merchant = db.MerchantViewModels.FirstOrDefault(i => i.UserId.Equals(merchantviewmodel.UserId));
                merchant.ImageUrl = merchantviewmodel.ImageUrl;
                merchant.WebsiteUrl = merchantviewmodel.WebsiteUrl;
                merchant.WebsiteName = merchantviewmodel.WebsiteName;
                merchant.WebsiteDescription = merchantviewmodel.WebsiteDescription;
                merchant.VendorSelect = merchantviewmodel.VendorSelect;
                merchant.VendorDetails = merchantviewmodel.VendorDetails;
                merchant.EmailAddress = merchantviewmodel.EmailAddress;
                merchant.Approved = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(merchantviewmodel);
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
