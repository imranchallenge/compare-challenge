using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompareIt4meChallenge.Models;

namespace CompareIt4meChallenge.Controllers
{
    public class CustomerPhoneController : Controller
    {
        private ChallengeDb db = new ChallengeDb();

        private CustomerRepository _db = new CustomerRepository();

        //
        // GET: /CustomerPhone/

        public ActionResult Index(string searchTerm = null)
        {
            int customerId;
            IList<CustomerPhone> model;

            if (searchTerm != null && int.TryParse(searchTerm, out customerId))
               model = _db.GetAllPhoneByCustomer(customerId);
            else
            {
                model = _db.GetAllPhones();
            }

            return View(model);
        }

        // GET: /CustomerPhone/Create

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerPhone customerphone)
        {
            if (ModelState.IsValid)
            {
                db.CustomerPhones.Add(customerphone);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customerphone);
        }
        
        // GET: /CustomerPhone/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CustomerPhone customerphone = _db.GetCustomerPhone(id);
            if (customerphone == null)
            {
                return HttpNotFound();
            }
            return View(customerphone);
        }

        //
        // POST: /CustomerPhone/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerPhone customerphone)
        {
            if (ModelState.IsValid)
            {
                _db.ActivatePhoneNumber(customerphone.CustomerId, customerphone.Id);
                return RedirectToAction("Index");
            }
            return View(customerphone);
        }

        protected override void Dispose(bool disposing)
        {
            if (_db != null)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}