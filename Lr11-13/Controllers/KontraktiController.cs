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
    public class KontraktiController : Controller
    {
        private TheaterEntities db = new TheaterEntities();

        // GET: Kontrakti
        public ActionResult Index(string searchString)
        {
            var kontrakti = from s in db.Контракты
                             select s;
            
            kontrakti = db.Контракты.Include(к => к.Актеры);

            if(!String.IsNullOrEmpty(searchString))
            {
                kontrakti = kontrakti.Where(s => s.Актеры.Сотрудники.ФИО.Contains(searchString));
            }
            
            return View(kontrakti.ToList());
        }

        // GET: Kontrakti/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Контракты контракты = db.Контракты.Find(id);
            if (контракты == null)
            {
                return HttpNotFound();
            }
            return View(контракты);
        }

        // GET: Kontrakti/Create
        public ActionResult Create()
        {
            List<Сотрудники> сотрудники = new List<Сотрудники>();
            foreach(var item in db.Актеры)
            {
                сотрудники.Add(item.Сотрудники);
            }
            ViewBag.id_актера = new SelectList(сотрудники,"id_сотрудника","ФИО");
            //ViewBag.id_актера = new SelectList(db.Актеры, "id_сотрудника", "Стаж_работы");
            return View();
        }

        // POST: Kontrakti/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_контракта,id_актера,Дата_заключения_контракта,Название_постановки,Дата_окончания_контракта,Выплаты_в_месяц")] Контракты контракты)
        {
            if (ModelState.IsValid)
            {
                db.Контракты.Add(контракты);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_актера = new SelectList(db.Актеры, "id_сотрудника", "Стаж_работы", контракты.id_актера);
            return View(контракты);
        }

        // GET: Kontrakti/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Контракты контракты = db.Контракты.Find(id);
            if (контракты == null)
            {
                return HttpNotFound();
            }
            List<Сотрудники> сотрудники = new List<Сотрудники>();
            foreach(var item in db.Актеры)
            {
                сотрудники.Add(item.Сотрудники);
            }
            ViewBag.id_актера = new SelectList(сотрудники,"id_сотрудника","ФИО",контракты.id_актера);
            //ViewBag.id_актера = new SelectList(db.Актеры, "id_сотрудника", "Стаж_работы", контракты.id_актера);
            return View(контракты);
        }

        // POST: Kontrakti/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_контракта,id_актера,Дата_заключения_контракта,Название_постановки,Дата_окончания_контракта,Выплаты_в_месяц")] Контракты контракты)
        {
            if (ModelState.IsValid)
            {
                db.Entry(контракты).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_актера = new SelectList(db.Актеры, "id_сотрудника", "Стаж_работы", контракты.id_актера);
            return View(контракты);
        }

        // GET: Kontrakti/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Контракты контракты = db.Контракты.Find(id);
            if (контракты == null)
            {
                return HttpNotFound();
            }
            return View(контракты);
        }

        // POST: Kontrakti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Контракты контракты = db.Контракты.Find(id);
            db.Контракты.Remove(контракты);
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
