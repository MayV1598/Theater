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
    public class AkteriController : Controller
    {
        private TheaterEntities db = new TheaterEntities();

        // GET: Akteri
        public ActionResult Index()
        {
            var актеры = db.Актеры.Include(а => а.Сотрудники);
            return View(актеры.ToList());
        }

        // GET: Akteri/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Актеры актеры = db.Актеры.Find(id);
            if (актеры == null)
            {
                return HttpNotFound();
            }
            return View(актеры);
        }

        // GET: Akteri/Create
        public ActionResult Create()
        {
            ViewBag.id_сотрудника = new SelectList(db.Сотрудники, "id_сотрудника", "ФИО");
            return View();
        }

        // POST: Akteri/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_сотрудника,Стаж_работы,Последняя_постановка")] Актеры актеры)
        {
            if (ModelState.IsValid)
            {
                db.Актеры.Add(актеры);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_сотрудника = new SelectList(db.Сотрудники, "id_сотрудника", "ФИО", актеры.id_сотрудника);
            return View(актеры);
        }

        // GET: Akteri/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Актеры актеры = db.Актеры.Find(id);
            if (актеры == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_сотрудника = new SelectList(db.Сотрудники, "id_сотрудника", "ФИО", актеры.id_сотрудника);
            return View(актеры);
        }

        // POST: Akteri/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_сотрудника,Стаж_работы,Последняя_постановка")] Актеры актеры)
        {
            if (ModelState.IsValid)
            {
                db.Entry(актеры).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_сотрудника = new SelectList(db.Сотрудники, "id_сотрудника", "ФИО", актеры.id_сотрудника);
            return View(актеры);
        }

        // GET: Akteri/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Актеры актеры = db.Актеры.Find(id);
            if (актеры == null)
            {
                return HttpNotFound();
            }
            return View(актеры);
        }

        // POST: Akteri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Актеры актеры = db.Актеры.Find(id);
            db.Актеры.Remove(актеры);
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
