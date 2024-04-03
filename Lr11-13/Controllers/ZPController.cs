using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Lr11_13.Models;
using OfficeOpenXml;

namespace Lr11_13.Controllers
{
    public class ZPController : Controller
    {
        private TheaterEntities db = new TheaterEntities();

        // GET: ZP
        public ActionResult Index()
        {
            var зП_актеров = db.ЗП_актеров.Include(з => з.Контракты);
            return View(зП_актеров.ToList());
        }

        // GET: ZP/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ЗП_актеров зП_актеров = db.ЗП_актеров.Find(id);
            if (зП_актеров == null)
            {
                return HttpNotFound();
            }
            return View(зП_актеров);
        }

        // GET: ZP/Create
        public ActionResult Create()
        {
            ViewBag.id_контракта = new SelectList(db.Контракты, "id_контракта", "Название_постановки");
            return View();
        }

        // POST: ZP/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_контракта,Месяц,Премия,Надбавка,Итог")] ЗП_актеров зП_актеров)
        {
            if (ModelState.IsValid)
            {
                db.ЗП_актеров.Add(зП_актеров);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_контракта = new SelectList(db.Контракты, "id_контракта", "Название_постановки", зП_актеров.id_контракта);
            return View(зП_актеров);
        }

        // GET: ZP/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ЗП_актеров зП_актеров = db.ЗП_актеров.Find(id);
            if (зП_актеров == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_контракта = new SelectList(db.Контракты, "id_контракта", "Название_постановки", зП_актеров.id_контракта);
            return View(зП_актеров);
        }

        // POST: ZP/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_контракта,Месяц,Премия,Надбавка,Итог")] ЗП_актеров зП_актеров)
        {
            if (ModelState.IsValid)
            {
                db.Entry(зП_актеров).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_контракта = new SelectList(db.Контракты, "id_контракта", "Название_постановки", зП_актеров.id_контракта);
            return View(зП_актеров);
        }

        // GET: ZP/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ЗП_актеров зП_актеров = db.ЗП_актеров.Find(id);
            if (зП_актеров == null)
            {
                return HttpNotFound();
            }
            return View(зП_актеров);
        }

        // POST: ZP/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ЗП_актеров зП_актеров = db.ЗП_актеров.Find(id);
            db.ЗП_актеров.Remove(зП_актеров);
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

        public FileResult Upload()
        {

            var zp = db.ЗП_актеров.ToList();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using(MemoryStream memoryStream = new MemoryStream())
            {
                using(ExcelPackage package = new ExcelPackage(memoryStream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Название листа");

                    // Записываем заголовки
                    worksheet.Cells["A1"].Value = "id";
                    worksheet.Cells["B1"].Value = "ФИО";
                    worksheet.Cells["C1"].Value = "Название постановки";
                    worksheet.Cells["D1"].Value = "Месяц";
                    worksheet.Cells["E1"].Value = "Премия";
                    worksheet.Cells["F1"].Value = "Надбавка";
                    worksheet.Cells["G1"].Value = "Итог";

                    // Заполняем данные
                    int row = 2;
                    foreach(var zpact in zp)
                    {
                        worksheet.Cells[row,1].Value = zpact.id;
                        worksheet.Cells[row,2].Value = zpact.Контракты.Актеры.Сотрудники.ФИО;
                        worksheet.Cells[row,3].Value = zpact.Контракты.Название_постановки;
                        worksheet.Cells[row,4].Value = zpact.Месяц;
                        worksheet.Cells[row,5].Value = zpact.Премия;
                        worksheet.Cells[row,6].Value = zpact.Надбавка;
                        worksheet.Cells[row,7].Value = zpact.Итог;

                        row++;
                    }

                    // Добавляем рамку вокруг таблицы
                    var range = worksheet.Cells["A1:G" + (row - 1)];
                    var border = range.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    // Автоширина столбцов
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    package.Save();
                }
                byte[] file = memoryStream.ToArray();
                string file_type = "application/xlsx";
                string file_name = "ЗП_актеров.xlsx";


                return File(file,file_type,file_name);
            }
        }
    }
}
