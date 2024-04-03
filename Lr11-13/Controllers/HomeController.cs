using Lr11_13.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Lr11_13.Controllers
{
    public class HomeController:Controller
    {
        private TheaterEntities db = new TheaterEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About(string start,string end)
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

            //ViewBag.Message = "";
            //
            //return View();
        }
    }
}