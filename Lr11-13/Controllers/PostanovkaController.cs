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
    public class PostanovkaController : Controller
    {
        private TheaterEntities db = new TheaterEntities();

        // GET: Postanovka
        public ActionResult Index(string searchString)
        {
            var postanovka = from s in db.Постановка
                            select s;

            if(!String.IsNullOrEmpty(searchString))
            {
                postanovka = postanovka.Where(s => s.Название_постановки.Contains(searchString));
            }
            return View(postanovka.ToList());
        }

        // GET: Postanovka/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Постановка постановка = db.Постановка.Find(id);
            if (постановка == null)
            {
                return HttpNotFound();
            }
            return View(постановка);
        }

        // GET: Postanovka/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Postanovka/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_постановки,Название_постановки,Описание,Длительность_постановки,Постер")] Постановка постановка,HttpPostedFileBase upload)
        {

            if(ModelState.IsValid)
            {
                if(upload != null && upload.ContentLength > 0)
                {
                    using(var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        постановка.Постер = reader.ReadBytes(upload.ContentLength);
                    }
                }
                db.Постановка.Add(постановка);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(постановка);
        }

        // GET: Postanovka/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Постановка постановка = db.Постановка.Find(id);
            if (постановка == null)
            {
                return HttpNotFound();
            }
            return View(постановка);
        }

        // POST: Postanovka/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_постановки,Название_постановки,Описание,Длительность_постановки,Постер")] Постановка постановка,HttpPostedFileBase upload)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    db.Entry(постановка).State = EntityState.Modified;

                    if(upload != null && upload.ContentLength > 0)
                    {
                        using(var reader = new System.IO.BinaryReader(upload.InputStream))
                        {
                            постановка.Постер = reader.ReadBytes(upload.ContentLength);
                        }
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Entry(постановка).Property(m => m.Постер).IsModified = false;
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index");
                }

                return View(постановка);
            }
            catch(Exception e) { return View(постановка); }
        }


        // GET: Postanovka/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Постановка постановка = db.Постановка.Find(id);
            if (постановка == null)
            {
                return HttpNotFound();
            }
            return View(постановка);
        }

        // POST: Postanovka/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Постановка постановка = db.Постановка.Find(id);
            db.Постановка.Remove(постановка);
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
