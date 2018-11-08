using RSSproject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using System.Xml.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Threading.Tasks;

namespace RSSproject.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [OutputCache(Duration = 60, Location = OutputCacheLocation.Server)]
        public ActionResult Index()
        {
            List<MainResource> resources = db.MainResources.
                Include(c => c.MainCollection).ToList();

            var gr = resources.GroupBy(r => r.MainCollection_Id);
            IEnumerable<MainResource> gropedResources = gr.SelectMany(g => g);


            List<MainCollection> collections = db.MainCollections.Include(c => c.MainResources).ToList();
            
            return View(collections);
        }

        private IQueryable<MainCollection> GetAllCollections()
        {
            return db.MainCollections.AsQueryable();
        }

        private async Task<List<MainCollection>> GetAllColl()
        {
            return await GetAllCollections().ToListAsync();
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

        [HttpGet]
        public ActionResult Resource(string RSSURL, string RSSName)
        {
            if (RSSURL == null) return RedirectToAction("Index");

            WebClient wclient = new WebClient();
            string RSSData = wclient.DownloadString(RSSURL);

            XDocument xml = XDocument.Parse(RSSData);

            var RSSFeedData = (from x in xml.Descendants("item")
                               let bytesTitle = Encoding.Default.GetBytes(((string)x.Element("title")))
                               let bytesDesc = Encoding.Default.GetBytes(((string)x.Element("description")))
                               
            select new RSSFeed
                               {
                                   Title = Encoding.UTF8.GetString(bytesTitle),
                                   Link = ((string)x.Element("link")),
                                   Description = Encoding.UTF8.GetString(bytesDesc),
                                   PubDate = ((string)x.Element("pubDate"))
                               });
            ViewBag.RSSFeed = RSSFeedData;
            ViewBag.URL = RSSURL;
            ViewBag.RSSName = RSSName;
            return View();
        }


        [HttpGet]
        public ActionResult AddResource()
        {
            ViewBag.collections = db.MainCollections.Include(c => c.MainResources).ToList();

            return View();
        }


        [HttpPost]
        public ActionResult AddResource(MainResource mainResource)
        {
            db.MainResources.Add(mainResource);
            db.SaveChanges();
            return RedirectToAction("Index");
            //return Content(mainResource.ResourceName.ToString());
        }
    }
}