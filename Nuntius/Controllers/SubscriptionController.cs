using Microsoft.AspNet.Identity;
using Nuntius.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Nuntius.Controllers
{
    public class SubscriptionController : Controller
    {
        // GET: Subscription
        public ActionResult Index()
        {
            string currentUserId = User.Identity.GetUserId();
            string downloadjson = "https://newsapi.org/v1/sources?language=en&apiKey=346e17ce990f4aacac337fe81afb6f50";
            WebClient c = new WebClient();
            var json = c.DownloadString(downloadjson);

            ApplicationDbContext db = new ApplicationDbContext();
            ApplicationUser currentUser = db.Users.FirstOrDefault(o => o.Id == currentUserId);
            AllSources allSources = Newtonsoft.Json.JsonConvert.DeserializeObject<AllSources>(json);

            foreach (var item in allSources.Sources) {
                if(!db.Sources.Any(o=>o.Id == item.Id))
                db.Sources.Add(item);
            }
            db.SaveChanges();
            return View(allSources.Sources);
        }

        [HttpPost]
        public ActionResult Index(List<string> sourceList) 
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(o => o.Id == currentUserId);

            foreach (var item in sourceList)
            {
                var sourceItem = db.Sources.Where(o => o.Id == item).FirstOrDefault();
                currentUser.Subscription.Sources.Add(sourceItem);

            }
            db.SaveChanges();
            return Redirect("Index");
        }

        public ActionResult ShowSubscribed()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(o => o.Id == currentUserId);

            return View(currentUser.Subscription.Sources);

        }
    }
}
