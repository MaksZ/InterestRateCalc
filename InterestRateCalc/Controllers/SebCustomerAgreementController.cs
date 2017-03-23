using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InterestRateCalc.DAL;
using InterestRateCalc.Models;
using System.Data.Entity.Infrastructure;

namespace InterestRateCalc.Controllers
{
    public class SebCustomerAgreementController: Controller
    {
        private SebContext db = new SebContext();

        // GET: Agrement
        public async Task<ActionResult> Index(int? SelectedCustomer)
        {
            var customers = db.Customers
                .OrderBy(q => q.LastName)
                .ThenBy(q => q.FirstName)
                .Select(x => new { Id = x.Id, Name = x.FirstName + " " + x.LastName})
                .ToList();

            int customerId = SelectedCustomer.GetValueOrDefault(1);

            ViewBag.SelectedCustomer = new SelectList(customers, "Id", "Name", customerId);

            var agreements = db.Agreements.Where(x => x.Customer.Id == customerId);
            return View(await agreements.ToListAsync());
        }

        // GET: Agrement/Create
        public ActionResult Create(int? Id)
        {
            if (Id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            if (!db.Customers.Any(x => x.Id == Id.Value))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            PopulateBaseRateCodes(null);
            return View(new ViewModels.NewAgreement { CustomerId = Id.Value } );
        }

        // POST: Agrement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePost(int? SelectedCustomer)
        {
            var vm = new ViewModels.NewAgreement();

            try
            {
                if (TryUpdateModel(vm, "", new string[] { "CustomerId", "Amount", "BaseRateCode", "Margin", "Duration" }))
                {
                    var masterCustomer = db.Customers.Find(vm.CustomerId);

                    if (masterCustomer != null)
                    {
                        var agreement = new SebCustomerAgreement
                        {
                            Amount = vm.Amount,
                            BaseRateCode = vm.BaseRateCode,
                            Margin = vm.Margin,
                            Duration = vm.Duration,
                            Customer = masterCustomer
                        };

                        db.Agreements.Add(agreement);
                        db.SaveChanges();

                        return RedirectToAction("Index", new { SelectedCustomer = masterCustomer.Id });
                    }
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            PopulateBaseRateCodes(null);
            return View(vm);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var agreement = db.Agreements.Include(x => x.Customer).First(x => x.Id == id);
            if (agreement == null)
            {
                return HttpNotFound();
            }

            ViewBag.SelectedCustomer = agreement.Customer.Id;
            PopulateBaseRateCodes(agreement.BaseRateCode);
            return View(agreement);
        }

        private void PopulateBaseRateCodes(string selectedValue)
        {
            var values = new[] { "VILIBOR1m", "VILIBOR3m", "VILIBOR6m", "VILIBOR1y" };
            ViewBag.BaseRateCode = new SelectList(values, selectedValue ?? values[0]);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var agreement = db.Agreements.Include(x => x.Customer).First(x => x.Id == id);

            try
            {
                var oldBaseRateCode = agreement.BaseRateCode;

                if (TryUpdateModel(agreement, "", new string[] { "BaseRateCode" }))
                {
                    await db.SaveChangesAsync();

                    var report = await BLL.CalculationReport.Build(agreement, oldBaseRateCode);

                    return View("Report", ToViewModel(report));
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Something totally went wrong. Try again, and if the problem persists, see your system administrator.");
            }

            ViewBag.SelectedCustomer = agreement.Customer.Id;
            PopulateBaseRateCodes(agreement.BaseRateCode);
            return View(agreement);
        }

        // GET: SebCustomer/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            var entry = db.Agreements.Find(id);
            if (entry == null)
            {
                return HttpNotFound();
            }
            return View(entry);
        }

        // POST: SebCustomer/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                var entry = db.Agreements.Find(id);
                db.Agreements.Remove(entry);
                db.SaveChanges();
            }
            catch (RetryLimitExceededException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
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

        private static ViewModels.CalculationReport ToViewModel(BLL.CalculationReport report)
            =>
            new ViewModels.CalculationReport
            {
                CustomerAgreementDescription = report.CustomerAgreementDescription,
                CurrentBaseRate = report.CurrentBaseRateCode,
                CurrentInterestRate = report.CurrentInterestRate,
                NewBaseRate = report.NewBaseRateCode,
                NewInterestRate = report.NewInterestRate,
                Difference = report.Difference
            };
    }
}