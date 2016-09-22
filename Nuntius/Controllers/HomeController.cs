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

			var list = new List<string>(new[] { "bbc-news", "ars-technica", "associated-press","bbc-sport",  "bloomberg",
				"business-insider", "business-insider-uk", "buzzfeed", "cnbc", "cnn", "daily-mail", "engadget", "entertainment-weekly",
				"espn", "financial-times", "focus", "google-news", "ign", "independent", "mashable", "metro", "mirror", "new-scientist",
				"newsweek","reuters", "sky-news", "techcrunch", "techradar", "the-guardian-uk", "the-huffington-post", "the-new-york-times",
				"the-telegraph", "the-verge", "the-wall-street-journal", "the-washington-post", "time"});
			var rng = new Random();
			var randomElement = list[rng.Next(list.Count)];
			string source = randomElement;
			WebClient c = new WebClient();
			string downloadjson = "https://newsapi.org/v1/articles?source=" + source + /*"&sortBy="+sortedby+*/
								  "&apiKey=346e17ce990f4aacac337fe81afb6f50";
			var json =
				c.DownloadString(downloadjson);
			Newsheadline newsheadline = Newtonsoft.Json.JsonConvert.DeserializeObject<Newsheadline>(json);
			@ViewBag.Source = newsheadline.Source;
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