using Nuntius.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Nuntius.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string id)
        {

            string source = "nosource";

            string sortedby = "latest";
            if (id == null)
            {
                source = "the-next-web";
            }
            else if (id == "bbc-news" || id == "google-news")
            {
                source = id;
                sortedby = "popular";
            }
            else
            {
                source = id;
            }
            WebClient c = new WebClient();
            string downloadjson = "https://newsapi.org/v1/articles?source=" + source + /*"&sortBy="+sortedby+*/
                                  "&apiKey=346e17ce990f4aacac337fe81afb6f50";
            var json =
                c.DownloadString(downloadjson);


            Newsheadline newsheadline = Newtonsoft.Json.JsonConvert.DeserializeObject<Newsheadline>(json);



            return View(newsheadline);


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
    }
}