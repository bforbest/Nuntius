﻿using Microsoft.AspNet.Identity;
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
        ApplicationDbContext db = new ApplicationDbContext();
        ApplicationUser currentUser = new ApplicationUser();
        WebClient c = new WebClient();
        public ActionResult Index()
        {
            string currentUserId = User.Identity.GetUserId();
            currentUser = db.Users.FirstOrDefault(o => o.Id == currentUserId);
         
            string downloadjson = "https://newsapi.org/v1/sources?language=en&apiKey=346e17ce990f4aacac337fe81afb6f50";
            var json =
                c.DownloadString(downloadjson);
            AllSources allSources = Newtonsoft.Json.JsonConvert.DeserializeObject<AllSources>(json);
            foreach (var item in allSources.Sources)
            {
                if(!db.Sources.Any(o=>o.Id == item.Id))
                db.Sources.Add(item);
            }
            db.SaveChanges();
            if (currentUser.Subscription != null)
            {
                IList<string> sources = new List<string>();
                foreach (var item in currentUser.Subscription.Sources)
                {
                     sources.Add(item.Id);
                }
                ViewBag.Usersubs = sources;
            }
         
            return View(allSources.Sources);
        }

        [HttpPost]
        public ActionResult Index(List<string> sourceList)
        {
           
            string currentUserId = User.Identity.GetUserId();
            currentUser = db.Users.FirstOrDefault(o => o.Id == currentUserId);
            var l = currentUser.Subscription.Sources;
            foreach (var item in sourceList)
            {
                var subItem = currentUser.Subscription.Sources.FirstOrDefault(a => a.Id == item);
                if (subItem != null && subItem.Id == item)
                {
                   
                    currentUser.Subscription.Sources.Remove(subItem);
                }
                else
                {
                    var sourceItem = db.Sources.Where(o => o.Id == item).FirstOrDefault();
                    currentUser.Subscription.Sources.Add(sourceItem);

                }
            }
            @ViewBag.SavedSubscription = "Your subscription has been updated and saved";
            db.SaveChanges();  
            return Redirect("/Subscription/Index");
        }

        public ActionResult ShowSubscribed()
        {
            
            string currentUserId = User.Identity.GetUserId();
            currentUser = db.Users.FirstOrDefault(o => o.Id == currentUserId);

            return View(currentUser.Subscription.Sources);

        }
        public ActionResult _Subscribednews()
        {
            
            string currentUserId = User.Identity.GetUserId();
            currentUser = db.Users.FirstOrDefault(o => o.Id == currentUserId);
            
              IList<Newsheadline> Articles = new List<Newsheadline>();
            
            foreach (var item in currentUser.Subscription.Sources)
            {
                string downloadjson = "https://newsapi.org/v1/articles?source=" + item.Id + /*"&sortBy="+sortedby+*/
                                  "&apiKey=346e17ce990f4aacac337fe81afb6f50";
                var json =
                    c.DownloadString(downloadjson);
                Newsheadline newsheadline = Newtonsoft.Json.JsonConvert.DeserializeObject<Newsheadline>(json);

                Articles.Add(newsheadline);
            }
            return View(Articles);

        }
    }
}
