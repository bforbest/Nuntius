/* File: Controller Class: ['ArticleController']
// Project: Nuntius - The independent news aggeregator
// Credits: Mustafa, Omran, Ali, Jimmy
// Description: The 'fair' customizable independent news provider
// Last Update: 04:24
*/

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
        WebClient client = new WebClient();
        Source source = new Source();


        #region # ActionResult() Home/Index
        public ActionResult Index(string id, WebClient client) {


			var list = new List<string>(new[] { "bbc-news"  /*"associated-press","bbc-sport",  "bloomberg",
				"business-insider", "business-insider-uk", "buzzfeed", "cnbc", "cnn", "daily-mail", "engadget", "entertainment-weekly",
				"espn", "financial-times", "focus", "google-news", "ign", "independent", "mashable", "metro", "mirror", "new-scientist",
				"newsweek","reuters", "sky-news", "techcrunch", "techradar", "the-guardian-uk", "the-huffington-post", "the-new-york-times",
				"the-telegraph", "the-verge", "the-wall-street-journal", "the-washington-post", "time"
                
                */});

            var rng = new Random();
			var randomSourceElement = list[rng.Next(list.Count)];
			string source = randomSourceElement;
            

            string downloadjson = "https://newsapi.org/v1/articles?source=" + source + /*"&sortBy="+sortedby+*/
								  "&apiKey=346e17ce990f4aacac337fe81afb6f50";
			var json = client.DownloadString(downloadjson);
			Newsheadline newsheadline = Newtonsoft.Json.JsonConvert.DeserializeObject<Newsheadline>(json);

            var countsource = context.Sources;
		        if (countsource.Count() <= 1) {
                    FillDataBasewithSource(client);
		        }
                Source Source = context.Sources.FirstOrDefault(a => a.Id == source);
		        NewsSource newsource = new NewsSource() {Newsheadline = newsheadline, Source = Source};
            return View(newsource);
		}
        #endregion

        public void FillDataBasewithSource(WebClient client) {
            client = new WebClient();
            string newsApiUrl = "https://newsapi.org/v1/sources?language=en&apiKey=346e17ce990f4aacac337fe81afb6f50";
            var json2 = client.DownloadString(newsApiUrl);
            AllSources RootObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AllSources>(json2);

            var p = context.Sources;

            if (p.Count() <= 1)
            {
                foreach (var item in RootObject.Sources)
                {
                    source.Id = item.Id;
                    source.Name = item.Name;
                    source.SortBysAvailable = item.SortBysAvailable;
                    source.Category = item.Category;
                    source.Country = item.Country;
                    source.Description = item.Description;
                    source.Language = item.Language;
                    source.Url = item.Url;
                    source.UrlsToLogos = item.UrlsToLogos;
                    context.Sources.Add(source);
                };
                    context.SaveChanges();
            }
        }

        
        #region # ActionResult() Home/Contact
        public ActionResult Contact()
        {
            //GET Home/Contact
            ViewBag.ContactEmail = "";
            ViewBag.ContactPhone = "";
            ViewBag.ContactAddress = "";
            return View();
        }
        #endregion

        #region # ActionResult() Home/About
        public ActionResult About()
        {
            ViewBag.test = "";
            return View();
        }
        #endregion


        public ActionResult Top(WebClient client)
        {
            var list = new List<string>(new[] { "bbc-news", /*"associated-press","bbc-sport",  "bloomberg",
				"business-insider", "business-insider-uk", "buzzfeed", "cnbc", "cnn", "daily-mail", "engadget", "entertainment-weekly",
				"espn", "financial-times", "focus", "google-news", "ign", "independent", "mashable", "metro", "mirror", "new-scientist",
				"newsweek","reuters", "sky-news", "techcrunch", "techradar", "the-guardian-uk", "the-huffington-post", "the-new-york-times",
				"the-telegraph", "the-verge", "the-wall-street-journal", "the-washington-post", "time"*/});


            client = new WebClient();
            string newsApiUrl = "https://newsapi.org/v1/sources?language=en&apiKey=346e17ce990f4aacac337fe81afb6f50";
            var json2 = client.DownloadString(newsApiUrl);
            AllSources RootObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AllSources>(json2);


            var rng = new Random();
            var randomElement = list[rng.Next(list.Count)];
            string source = randomElement;
            
            string downloadjson = "https://newsapi.org/v1/articles?source=" + source + /*"&sortBy="+sortedby+*/
                                  "&apiKey=346e17ce990f4aacac337fe81afb6f50";
            var json = client.DownloadString(downloadjson);
            Newsheadline newsheadline = Newtonsoft.Json.JsonConvert.DeserializeObject<Newsheadline>(json);
            @ViewBag.Source = newsheadline.Source;
            return View(newsheadline);
        }

    }

}
