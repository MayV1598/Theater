using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Lr11_13.Models;

namespace Lr11_13.Controllers
{
    public class RoliController : Controller
    {
        private TheaterEntities db = new TheaterEntities();

        // GET: Roli
        public ActionResult Index(string searchString,string searchString1)
        {
            var roli = from s in db.Роли_актеров
                            select s;

            roli = db.Роли_актеров.Include(р => р.Актеры).Include(р => р.Постановка);

            if(!String.IsNullOrEmpty(searchString))
            {
                roli = roli.Where(s => s.Актеры.Сотрудники.ФИО.Contains(searchString));
            }
            if(!String.IsNullOrEmpty(searchString1))
            {
                roli = roli.Where(s => s.Название_роли.Contains(searchString1));
            }

            return View(roli.ToList());
        }

        // GET: Roli/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Роли_актеров роли_актеров = db.Роли_актеров.Find(id);
            if (роли_актеров == null)
            {
                return HttpNotFound();
            }
            return View(роли_актеров);
        }

        // GET: Roli/Create
        public ActionResult Create()
        {
            //ViewBag.id_актера = new SelectList(db.Актеры, "id_сотрудника", "Стаж_работы");
            List<Сотрудники> сотрудники = new List<Сотрудники>();
            foreach(var item in db.Актеры)
            {
                сотрудники.Add(item.Сотрудники);
            }
            ViewBag.id_актера = new SelectList(сотрудники, "id_сотрудника", "ФИО");

            ViewBag.id_постановки = new SelectList(db.Постановка, "id_постановки", "Название_постановки");
            return View();
        }

        // POST: Roli/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_роли,Название_роли,id_актера,id_постановки")] Роли_актеров роли_актеров)
        {
            if (ModelState.IsValid)
            {
                db.Роли_актеров.Add(роли_актеров);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_актера = new SelectList(db.Актеры, "id_сотрудника", "Стаж_работы", роли_актеров.id_актера);
            ViewBag.id_постановки = new SelectList(db.Постановка, "id_постановки", "Название_постановки", роли_актеров.id_постановки);
            return View(роли_актеров);
        }

        // GET: Roli/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Роли_актеров роли_актеров = db.Роли_актеров.Find(id);
            if (роли_актеров == null)
            {
                return HttpNotFound();
            }
            List<Сотрудники> сотрудники = new List<Сотрудники>();
            foreach(var item in db.Актеры)
            {
                сотрудники.Add(item.Сотрудники);
            }
            ViewBag.id_актера = new SelectList(сотрудники,"id_сотрудника","ФИО", роли_актеров.id_актера);
            //ViewBag.id_актера = new SelectList(db.Актеры, "id_сотрудника", "Стаж_работы", роли_актеров.id_актера);
            ViewBag.id_постановки = new SelectList(db.Постановка, "id_постановки", "Название_постановки", роли_актеров.id_постановки);
            return View(роли_актеров);
        }

        // POST: Roli/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_роли,Название_роли,id_актера,id_постановки")] Роли_актеров роли_актеров)
        {
            if (ModelState.IsValid)
            {
                db.Entry(роли_актеров).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_актера = new SelectList(db.Актеры, "id_сотрудника", "Стаж_работы", роли_актеров.id_актера);
            ViewBag.id_постановки = new SelectList(db.Постановка, "id_постановки", "Название_постановки", роли_актеров.id_постановки);
            return View(роли_актеров);
        }

        // GET: Roli/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Роли_актеров роли_актеров = db.Роли_актеров.Find(id);
            if (роли_актеров == null)
            {
                return HttpNotFound();
            }
            return View(роли_актеров);
        }

        // POST: Roli/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Роли_актеров роли_актеров = db.Роли_актеров.Find(id);
            db.Роли_актеров.Remove(роли_актеров);
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
