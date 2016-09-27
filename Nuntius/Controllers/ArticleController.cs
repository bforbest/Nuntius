using Nuntius.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Microsoft.AspNet.Identity;
using Nuntius.Helpers;

namespace Nuntius.Controllers
{
    public class ArticleController : Controller
    {
        WebClient c = new WebClient();
        ApplicationDbContext context = new ApplicationDbContext();
        DbActions action = new DbActions();
        // GET: Article
        public ActionResult Index(string id, string source)
		{
            
            string downloadjson = "https://newsapi.org/v1/articles?source=" + source +
                            "&apiKey=346e17ce990f4aacac337fe81afb6f50";
            var json =
              c.DownloadString(downloadjson);
            Newsheadline newsheadline = Newtonsoft.Json.JsonConvert.DeserializeObject<Newsheadline>(json);
            var x = newsheadline.Articles.FirstOrDefault(a => helperfunctions.hashing(a.Title) == id);

            var sourcearticle = context.Sources.FirstOrDefault(a => a.Id == source);
            ArticleSource newssource = new ArticleSource()
            {
                Article = x,
                Source = sourcearticle
            };
		    var currentuser = User.Identity.GetUserId();
            @ViewBag.User = context.Users.FirstOrDefault(a => a.Id == currentuser);
            return View(newssource);
        }


		// GET: Article/Details/5
		public ActionResult Details()
		{
		    return View();
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
            var xArticle = newsheadline.Articles.FirstOrDefault(a => helperfunctions.hashing(a.Title) == id);
            if (xArticle.Author == null)
            {
                xArticle.Author = "Unknown";
            }

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
        [HttpPost]
        public ActionResult AddComment(string urlId, String comment, string articleSource)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            WebClient c = new WebClient();
            //This makes sure it uses the real source and thus duplicates of same titles are to be very rare
            string downloadjson = "https://newsapi.org/v1/articles?source=" + articleSource +
                            "&apiKey=346e17ce990f4aacac337fe81afb6f50";
            var json =
              c.DownloadString(downloadjson);
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = context.Users.FirstOrDefault(o => o.Id == currentUserId);
            Newsheadline newsheadline = Newtonsoft.Json.JsonConvert.DeserializeObject<Newsheadline>(json);            
            var article = newsheadline.Articles.Where(o=>o.Url==urlId).FirstOrDefault();
            //Check if the article is already in the database if not save it in database
            if (context.Articles.Any(o => o.Url != article.Url))
            {
                article.Source = context.Sources.Where(o => o.Id == articleSource).FirstOrDefault();
                var savedArticle = context.Articles.Add(article);
                context.Comments.Add(new Comment { CommentText = comment, Article = article, DatePublished = DateTime.Now, User = currentUser });
                context.SaveChanges();
            }
            return Json(comment);
        }

        public ActionResult ShowComments(string url)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            db.Configuration.ProxyCreationEnabled = false;
            var comments = db.Comments.Include("User").Where(o => o.Article.Url == url);
            return Json(comments.ToList());
        }
        public ActionResult DeleteComment(int id, string userId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string currentUserId = User.Identity.GetUserId();

            db.Configuration.ProxyCreationEnabled = false;
            string jsonMessage = "Nothing happened";
            if (userId == currentUserId)
            {
                Comment comment = db.Comments.FirstOrDefault(o => o.CommentId == id);
                
                db.Comments.Remove(comment);
                db.SaveChanges();
                jsonMessage = "Deleted";
            }
            else
            {
                jsonMessage = "You can not delete others comment";
            }
            return Json(jsonMessage);
        }
    }
}
