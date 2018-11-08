using RSSproject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace RSSproject.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            List<MainResource> resources = db.MainResources.
                Include(c => c.MainCollection).ToList();

            var gr = resources.GroupBy(r => r.MainCollection_Id);
            IEnumerable<MainResource> gropedResources = gr.SelectMany(g => g);
            
            return View(gropedResources.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        [HttpGet]
        public ActionResult AddCollection()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCollection(MainCollection collection)
        {
            if (collection != null)
            {
                db.MainCollections.Add(collection);
                db.SaveChanges();
            }


            return RedirectToAction("Index");
        }

    }
}