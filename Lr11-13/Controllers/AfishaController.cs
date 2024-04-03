using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Lr11_13.Models;
using Microsoft.Ajax.Utilities;

namespace Lr11_13.Controllers
{
    public class AfishaController : Controller
    {
        private TheaterEntities db = new TheaterEntities();

        // GET: Afisha
        public ActionResult Index(string start,string end)
        {

            List<Афиша> постановка;

            if(!start.IsNullOrWhiteSpace() && !end.IsNullOrWhiteSpace())
            {
                DateTime startDateTime = DateTime.Parse(start);
                DateTime endDateTime = DateTime.Parse(end);

                постановка = db.Афиша.Include(e => e.Роли_актеров)
                .Include(e => e.Постановка)
                .Where(e => e.Дата_и_время_проведения >= startDateTime)
                .Where(e => e.Дата_и_время_проведения <= endDateTime)
                .ToList();
            }
            else
            {
                постановка = db.Афиша.Include(e => e.Роли_актеров)
                .Include(e => e.Постановка)
                .ToList();
            }

            //var афиша = db.Афиша.Include(а => а.Роли_актеров).Include(а => а.Постановка);
            //return View(афиша.ToList());
            return View(постановка);
         }

        //public ActionResult Index(string start, string end)
        //{
        //    List<Афиша> history;
        //    if(!start.IsNullOrWhiteSpace() && end.IsNullOrWhiteSpace())
        //    {
        //        DateTime startDT = DateTime.Parse(start);
        //        DateTime endDT = DateTime.Parse(end);
        //        history = db.Афиша.Where(e => e.Дата_и_время_проведения >= startDT)
        //            .Where(e => e.Дата_и_время_проведения <= endDT).ToList();
        //    }
        //    else
        //    {
        //        history = db.Афиша.ToList();
        //    }
        //    return View(history);
        //}

        // GET: Afisha/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Афиша афиша = db.Афиша.Find(id);
            if (афиша == null)
            {
                return HttpNotFound();
            }
            return View(афиша);
        }

        // GET: Afisha/Create
        public ActionResult Create()
        {
            List<Сотрудники> сотрудники = new List<Сотрудники>();
            foreach(var item in db.Актеры)
            {
                сотрудники.Add(item.Сотрудники);
            }
            //ViewBag.id_актера = 
            //List<Актеры> актеры = new List<Актеры>();
            //foreach(var item in db.Роли_актеров)
            //{
            //    актеры.Add(item.Актеры);
            //}
            //ViewBag.id_актера = new SelectList(актеры,"id_сотрудника","ФИО");
            ViewBag.id_главной_роли = new SelectList(сотрудники,"id_сотрудника","ФИО");/*new SelectList(актеры, "id_роли", "Название_роли");*/
            ViewBag.id_постановки = new SelectList(db.Постановка, "id_постановки", "Название_постановки");
            return View();
        }

        // POST: Afisha/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_афиши,id_постановки,Кол_во_билетов,id_главной_роли,Цена_билета,Дата_и_время_проведения")] Афиша афиша)
        {
            if (ModelState.IsValid)
            {
                db.Афиша.Add(афиша);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_главной_роли = new SelectList(db.Роли_актеров, "id_роли", "Название_роли", афиша.id_главной_роли);
            ViewBag.id_постановки = new SelectList(db.Постановка, "id_постановки", "Название_постановки", афиша.id_постановки);
            return View(афиша);
        }

        // GET: Afisha/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Афиша афиша = db.Афиша.Find(id);
            if (афиша == null)
            {
                return HttpNotFound();
            }
            List<Сотрудники> сотрудники = new List<Сотрудники>();
            foreach(var item in db.Актеры)
            {
                сотрудники.Add(item.Сотрудники);
            }
            ViewBag.id_главной_роли = new SelectList(сотрудники,"id_сотрудника","ФИО");
            //ViewBag.id_главной_роли = new SelectList(db.Роли_актеров, "id_роли", "Название_роли", афиша.id_главной_роли);
            ViewBag.id_постановки = new SelectList(db.Постановка, "id_постановки", "Название_постановки", афиша.id_постановки);
            return View(афиша);
        }

        // POST: Afisha/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_афиши,id_постановки,Кол_во_билетов,id_главной_роли,Цена_билета,Дата_и_время_проведения")] Афиша афиша)
        {
            if (ModelState.IsValid)
            {
                db.Entry(афиша).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_главной_роли = new SelectList(db.Роли_актеров, "id_роли", "Название_роли", афиша.id_главной_роли);
            ViewBag.id_постановки = new SelectList(db.Постановка, "id_постановки", "Название_постановки", афиша.id_постановки);
            return View(афиша);
        }

        // GET: Afisha/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Афиша афиша = db.Афиша.Find(id);
            if (афиша == null)
            {
                return HttpNotFound();
            }
            return View(афиша);
        }

        // POST: Afisha/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Афиша афиша = db.Афиша.Find(id);
            db.Афиша.Remove(афиша);
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
