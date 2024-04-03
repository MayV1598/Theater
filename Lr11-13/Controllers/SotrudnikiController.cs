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
    public class SotrudnikiController : Controller
    {
        private TheaterEntities db = new TheaterEntities();

        // GET: Sotrudniki
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var sotrudniki = from s in db.Сотрудники
                             select s;

            if(!String.IsNullOrEmpty(searchString))
            {
                sotrudniki = sotrudniki.Where(s=>s.ФИО.Contains(searchString));
            }

            switch(sortOrder)
            {
                case "name_desc":
                    sotrudniki = sotrudniki.OrderByDescending(s => s.ФИО);
                    break;
                case "Date":
                    sotrudniki = sotrudniki.OrderBy(s => s.Дата_рождения);
                    break;
                case "date_desc":
                    sotrudniki = sotrudniki.OrderByDescending(s => s.Дата_рождения);
                    break;
                default:
                    sotrudniki = sotrudniki.OrderBy(s => s.ФИО);
                    break;
            }
            return View(sotrudniki.ToList());
        }

        // GET: Sotrudniki/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Сотрудники сотрудники = db.Сотрудники.Find(id);
            if (сотрудники == null)
            {
                return HttpNotFound();
            }
            return View(сотрудники);
        }

        // GET: Sotrudniki/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sotrudniki/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_сотрудника,ФИО,Дата_рождения,Номер_телефона,Должность,Логин,Пароль")] Сотрудники сотрудники)
        {
            if (ModelState.IsValid)
            {
                db.Сотрудники.Add(сотрудники);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(сотрудники);
        }

        // GET: Sotrudniki/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Сотрудники сотрудники = db.Сотрудники.Find(id);
            if (сотрудники == null)
            {
                return HttpNotFound();
            }
            return View(сотрудники);
        }

        // POST: Sotrudniki/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_сотрудника,ФИО,Дата_рождения,Номер_телефона,Должность,Логин,Пароль")] Сотрудники сотрудники)
        {
            if (ModelState.IsValid)
            {
                db.Entry(сотрудники).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(сотрудники);
        }

        // GET: Sotrudniki/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Сотрудники сотрудники = db.Сотрудники.Find(id);
            if (сотрудники == null)
            {
                return HttpNotFound();
            }
            return View(сотрудники);
        }

        // POST: Sotrudniki/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Сотрудники сотрудники = db.Сотрудники.Find(id);
            db.Сотрудники.Remove(сотрудники);
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
