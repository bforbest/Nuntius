using Nuntius.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Nuntius.Controllers
{
    public class ArticleController : Controller
    {
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

			return View(x);
		}

		// GET: Article/Create
		public ActionResult Create()
        {
            return View();
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
