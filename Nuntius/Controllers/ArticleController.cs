using Nuntius.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Nuntius.Controllers
{
    public class ArticleController : Controller
    {
        DbActions action = new DbActions();
        // GET: Article
        public ActionResult Index(string id, string source)
		{
			ApplicationDbContext context = new ApplicationDbContext();

			WebClient c = new WebClient();
			string downloadjson = "https://newsapi.org/v1/articles?source=" + source +
							"&apiKey=346e17ce990f4aacac337fe81afb6f50";
			var json =
			  c.DownloadString(downloadjson);
			Newsheadline newsheadline = Newtonsoft.Json.JsonConvert.DeserializeObject<Newsheadline>(json);
			var x = newsheadline.Articles.FirstOrDefault(a => a.Title.Split(' ').Last() == id);

			return View(x);
		}


		// GET: Article/Details/5
		public ActionResult Details(string id, string source)
		{
			ApplicationDbContext context = new ApplicationDbContext();

			WebClient c = new WebClient();
			string downloadjson = "https://newsapi.org/v1/articles?source=" + source +
							"&apiKey=346e17ce990f4aacac337fe81afb6f50";
			var json =
			  c.DownloadString(downloadjson);
			Newsheadline newsheadline = Newtonsoft.Json.JsonConvert.DeserializeObject<Newsheadline>(json);
			var x = newsheadline.Articles.FirstOrDefault(a => a.Title.Split(' ').Last() == id);
       
            var p = x;
            x.Source = context.Sources.FirstOrDefault(a => a.Id == source);
            @ViewBag.Source = x.Source.Id;
            return View(x);
		}

        // GET: Article/Create
        public ActionResult Create(string id, string source)
        {
            WebClient c = new WebClient();
            //This makes sure it uses the real source and thus duplicates of same titles are to be very rare
            string downloadjson = "https://newsapi.org/v1/articles?source=" + source +
                            "&apiKey=346e17ce990f4aacac337fe81afb6f50";
            var json =
              c.DownloadString(downloadjson);
            var currentUserId = User.Identity.GetUserId();
            Newsheadline newsheadline = Newtonsoft.Json.JsonConvert.DeserializeObject<Newsheadline>(json);
            //Finds the Article that is favorite based on the last index of the title
            var xArticle = newsheadline.Articles.FirstOrDefault(a => a.Title.Split(' ').Last() == id);


            action.SaveToFavorite(xArticle, source, currentUserId);


            return Redirect(Request.UrlReferrer.PathAndQuery);
        }

        // POST: Article/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Article/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Article/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Article/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Article/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult AddComment(int id, String comment)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            context.Comments.Add(new Comment { ArticleId = id, CommentText = comment });
            return Json(comment);
        }
    }
}
