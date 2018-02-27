using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bank_PiRIS.Models;

namespace Bank_PiRIS.Controllers
{
    public class ClientsController : Controller
    {
        private BankDBEntities db = new BankDBEntities();

        // GET: Clients
        public ActionResult Index()
        {
            var clients = db.Clients;
            return View(clients.ToList());
        }

        // GET: Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Client client = db.Clients.Find(id);

            client.City = db.Cities.Find(client.CityId);
            client.Disability1 = db.Disabilities.Find(client.Disability);
            client.FamilyStatu = db.FamilyStatus.Find(client.FamilyStatusId);
            client.Nationality = db.Nationalities.Find(client.NationalityId);
            client.Passport = db.Passports.Find(client.PassportId);

            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "Name");
            ViewBag.Disability = new SelectList(db.Disabilities, "Disability1", "Name");
            ViewBag.FamilyStatusId = new SelectList(db.FamilyStatus, "FamilyStatusId", "Name");
            ViewBag.NationalityId = new SelectList(db.Nationalities, "NationalityId", "Name");
            return View();
        }

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClientId,Surname,Name,Patronymic,Birthday,Gender,CurrentAddress,Phone,MobilePhone,Email,ResidencePermit,Pensioner,Income,PassportId,Passport,Disability,NationalityId,FamilyStatusId,CityId")] Client client)
        {
            if (CheckPassport(client.Passport))
            {
                ViewBag.CityId = new SelectList(db.Cities, "CityId", "Name", client.CityId);
                ViewBag.Disability = new SelectList(db.Disabilities, "Disability1", "Name", client.Disability);
                ViewBag.FamilyStatusId = new SelectList(db.FamilyStatus, "FamilyStatusId", "Name", client.FamilyStatusId);
                ViewBag.NationalityId = new SelectList(db.Nationalities, "NationalityId", "Name", client.NationalityId);
                ViewBag.Error = "Проверьте паспортные данные!";
                return View(client);
            }

            Passport passport = client.Passport;
            db.Passports.Add(passport);
            client.PassportId = passport.PassportId;

            if (ModelState.IsValid)
            {
                db.Clients.Add(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityId = new SelectList(db.Cities, "CityId", "Name", client.CityId);
            ViewBag.Disability = new SelectList(db.Disabilities, "Disability1", "Name", client.Disability);
            ViewBag.FamilyStatusId = new SelectList(db.FamilyStatus, "FamilyStatusId", "Name", client.FamilyStatusId);
            ViewBag.NationalityId = new SelectList(db.Nationalities, "NationalityId", "Name", client.NationalityId);

            return View(client);
        }

        private Client TrimFields(Client client)
        {
            client.Name = client.Name.Trim();
            client.Surname = client.Surname.Trim();
            client.Patronymic = client.Patronymic.Trim();
            return client;
        }

        private bool CheckPassport(Passport passport)
        {
            Passport passportIdentification = db.Passports
                                            .Where(b => b.Identification == passport.Identification)
                                            .FirstOrDefault();
            Passport passportSeries = db.Passports
                                            .Where(b => b.Series == passport.Series && b.Number == passport.Number)
                                            .FirstOrDefault();

            if (passportIdentification == null && passportSeries == null)
                return false;

            return true;
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "Name", client.CityId);
            ViewBag.Disability = new SelectList(db.Disabilities, "Disability1", "Name", client.Disability);
            ViewBag.FamilyStatusId = new SelectList(db.FamilyStatus, "FamilyStatusId", "Name", client.FamilyStatusId);
            ViewBag.NationalityId = new SelectList(db.Nationalities, "NationalityId", "Name", client.NationalityId);
            client.Passport = db.Passports.Find(client.PassportId);
            return View(client);
        }

        // POST: Clients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClientId,Surname,Name,Patronymic,Birthday,Gender,CurrentAddress,Phone,MobilePhone,Email,ResidencePermit,Pensioner,Income,PassportId,Passport,Disability,NationalityId,FamilyStatusId,CityId")] Client client)
        {
            if (ModelState.IsValid)
            {
                Passport passport = client.Passport;
                db.Entry(client).State = EntityState.Modified;
                db.Entry(passport).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "Name", client.CityId);
            ViewBag.Disability = new SelectList(db.Disabilities, "Disability1", "Name", client.Disability);
            ViewBag.FamilyStatusId = new SelectList(db.FamilyStatus, "FamilyStatusId", "Name", client.FamilyStatusId);
            ViewBag.NationalityId = new SelectList(db.Nationalities, "NationalityId", "Name", client.NationalityId);
            client.Passport = db.Passports.Find(client.PassportId);
            return View(client);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);

            client.City = db.Cities.Find(client.CityId);
            client.Disability1 = db.Disabilities.Find(client.Disability);
            client.FamilyStatu = db.FamilyStatus.Find(client.FamilyStatusId);
            client.Nationality = db.Nationalities.Find(client.NationalityId);
            client.Passport = db.Passports.Find(client.PassportId);

            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = db.Clients.Find(id);
            Passport passport = db.Passports.Find(client.PassportId);
            db.Clients.Remove(client);
            db.Passports.Remove(passport);
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
