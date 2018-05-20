using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CustomerContactManager.Models;

namespace CustomerContactManager.Controllers
{
    public class CustomersController : Controller
    {
        private CustomerContext db = new CustomerContext();

        // GET: Customers
        public async Task<ActionResult> Index()
        {
            return View(await db.Customers.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Latitude,Longitude,Name")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.Id = Guid.NewGuid().ToString();
                db.Customers.Add(customer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Latitude,Longitude,Name")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            List<CustomerContact> contactList = await db.CustomerContacts.Where(x => x.CustomerId == id).ToListAsync();
            foreach (CustomerContact item in contactList)
            {
                db.CustomerContacts.Remove(item);
            }

            Customer customer = await db.Customers.FindAsync(id);
            db.Customers.Remove(customer);
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

        public ActionResult Contacts(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<CustomerContact> contactList = db.CustomerContacts.Where(x => x.CustomerId == id).ToList();
            if (contactList.Count == 0)
                contactList.Add(new CustomerContact() { CustomerId = id, Name = "No records" });

            return View(contactList);
        }

        public ActionResult CreateContact(string id)
        {
            CustomerContact contact = new CustomerContact();
            contact.CustomerId = id;
            return View(contact);
        }

        [HttpPost, ActionName("CreateContact")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateContact(CustomerContact contact)
        {
            if (ModelState.IsValid)
            {
                contact.Id = Guid.NewGuid().ToString();
                db.CustomerContacts.Add(contact);
                await db.SaveChangesAsync();
                return RedirectToAction("Contacts", new { id = contact.CustomerId });
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> DeleteContact(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerContact contact = await db.CustomerContacts.FindAsync(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        [HttpPost, ActionName("DeleteContact")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteContactConfirmed(string id)
        {
            CustomerContact contact = await db.CustomerContacts.FindAsync(id);
            var customerId = contact.CustomerId;
            db.CustomerContacts.Remove(contact);
            await db.SaveChangesAsync();
            return RedirectToAction("Contacts", new { id = customerId });
        }

        public async Task<ActionResult> EditContact(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerContact contact = await db.CustomerContacts.FindAsync(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditContact([Bind(Include = "Id,Email,ContactNumber,Name,CustomerId")] CustomerContact contact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Contacts", new { id = contact.CustomerId });
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> ContactDetails(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerContact contact = await db.CustomerContacts.FindAsync(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }
    }
}
