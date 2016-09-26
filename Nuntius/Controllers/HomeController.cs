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
        ApplicationDbContext context = new ApplicationDbContext();
        WebClient c = new WebClient();
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
			
			string downloadjson = "https://newsapi.org/v1/articles?source=" + source + /*"&sortBy="+sortedby+*/
								  "&apiKey=346e17ce990f4aacac337fe81afb6f50";
			var json =
				c.DownloadString(downloadjson);
			Newsheadline newsheadline = Newtonsoft.Json.JsonConvert.DeserializeObject<Newsheadline>(json);

            var countsource = context.Sources;
		    if (countsource.Count() <= 1)
		    {
                FillDataBasewithSource();
		    }
            Source Source = context.Sources.FirstOrDefault(a => a.Id == source);
		    NewsSource newsource = new NewsSource() {Newsheadline = newsheadline, Source = Source};
            return View(newsource);
		}

		public void FillDataBasewithSource()
          {

          
            string downloadjson2 = "https://newsapi.org/v1/sources?language=en&apiKey=346e17ce990f4aacac337fe81afb6f50";
            var json2 =
                c.DownloadString(downloadjson2);

            AllSources RootObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AllSources>(json2);
         

            var p = context.Sources;
            if (p.Count() <= 1)
            {
                foreach (var item in RootObject.Sources)
                {
                    Source Saws = new Source();
                    Saws.Id = item.Id;

                    Saws.Name = item.Name;
                    Saws.SortBysAvailable = item.SortBysAvailable;
                    Saws.Category = item.Category;
                    Saws.Country = item.Country;
                    Saws.Description = item.Description;
                    Saws.Language = item.Language;
                    Saws.Url = item.Url;
                    Saws.UrlsToLogos = item.UrlsToLogos;
                    context.Sources.Add(Saws);

                };
                context.SaveChanges();
            }
        }

        public ActionResult Contact()
        {
            ViewBag.ContactEmail = "";
            ViewBag.ContactPhone = "";
            ViewBag.ContactAddress = "";

            return View();
        }
    }
}
