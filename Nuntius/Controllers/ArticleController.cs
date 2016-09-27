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
            //ApplicationDbContext context = new ApplicationDbContext();

            //WebClient c = new WebClient();
            //string downloadjson = "https://newsapi.org/v1/articles?source=" + source +
            //				"&apiKey=346e17ce990f4aacac337fe81afb6f50";
            //var json =
            //  c.DownloadString(downloadjson);
            //Newsheadline newsheadline = Newtonsoft.Json.JsonConvert.DeserializeObject<Newsheadline>(json);
            //var x = newsheadline.Articles.FirstOrDefault(a => a.Title.Split(' ').Last() == id);

            //return View(x);



          
            //Borde verkligen ändra source description/title split idén.
            //if (source == "")
            //{
            //    source = w;
            //}
            //      else if (!Request.Url.AbsoluteUri.Contains("source"))
            //{
            //    source = Request.UrlReferrer.AbsoluteUri.Split('=').Last();
            //      }

            string downloadjson = "https://newsapi.org/v1/articles?source=" + source +
                            "&apiKey=346e17ce990f4aacac337fe81afb6f50";
            var json = c.DownloadString(downloadjson);
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
            var xArticle = newsheadline.Articles.FirstOrDefault(a => a.Url.Split('/').Last() == id);


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
            String jsonMessage = "";
            if (!String.IsNullOrEmpty(comment))
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
                if (currentUser == null)
                {
                    jsonMessage = "You are not logged in";
                }
                else
                {
                    var article = newsheadline.Articles.Where(o => o.Url == urlId).FirstOrDefault();
                    //Check if the article is already in the database if not save it in database
                    var IsThereAnyArticle = context.Articles.Any(o => o.Url == article.Url);
                    article.Source = context.Sources.Where(o => o.Id == articleSource).FirstOrDefault();
                    if (!IsThereAnyArticle)
                    {

                        var savedArticle = context.Articles.Add(article);

                    }
                    context.Comments.Add(new Comment { CommentText = comment, Article = article, DatePublished = DateTime.Now, User = currentUser });
                    context.SaveChanges();
                    jsonMessage = "Comment is sent";
                }
            }
            else
            {
                jsonMessage = "Please write a comment";
            }
            return Json(jsonMessage);
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
        public ActionResult CommentUpvote(int id, string userId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string currentUserId = User.Identity.GetUserId();
            db.Configuration.ProxyCreationEnabled = false;
            int upvoteCount = 0;
            Comment comment = db.Comments.Include("VotingComment").Where(o => o.CommentId == id).FirstOrDefault();
            VotingComment votingComment = comment.VotingComment.FirstOrDefault(o => o.ApplicationUserID == currentUserId && o.CommentId == comment.CommentId);
            if (votingComment==null)
            {
                
                comment.VotingComment.Add(new VotingComment {  ApplicationUserID=currentUserId, CommentId = id, VoteValue =true});
                db.SaveChanges();

            }
            else if(!votingComment.VoteValue)
            {
                votingComment.VoteValue = true;
                db.SaveChanges();
            }
            int count = comment.VotingComment.Count();
            upvoteCount = comment.VotingComment.Where(o => o.VoteValue).Count();
            return Json(upvoteCount + '-' + count-upvoteCount);
        }
        public ActionResult CommentDownvote(int id, string userId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string currentUserId = User.Identity.GetUserId();
            db.Configuration.ProxyCreationEnabled = false;
            string upvoteCount;
            Comment comment = db.Comments.Include("VotingComment").Where(o => o.CommentId == id).FirstOrDefault();
            VotingComment votingComment = comment.VotingComment.FirstOrDefault(o => o.ApplicationUserID==currentUserId&&o.CommentId==comment.CommentId);
            if (votingComment == null)
            {
                comment.VotingComment.Add(new VotingComment { ApplicationUserID = currentUserId, CommentId = id, VoteValue = false });
                db.SaveChanges();
            }
            else if(votingComment.VoteValue)
            {
                votingComment.VoteValue = false;
                db.SaveChanges();
            }
            string count = comment.VotingComment.Where(o=>o.VoteValue==false).Count().ToString();
            upvoteCount = comment.VotingComment.Where(o => o.VoteValue).Count().ToString();
            return Json(upvoteCount + ":" + count);
        }
        public ActionResult CountVote(int id, string userId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string currentUserId = User.Identity.GetUserId();
            db.Configuration.ProxyCreationEnabled = false;
            string upvoteCount;
            Comment comment = db.Comments.Include("VotingComment").FirstOrDefault(o => o.CommentId == id);
            VotingComment votingComment = comment.VotingComment.FirstOrDefault(o => o.ApplicationUserID == currentUserId);
            string count = comment.VotingComment.Where(o => o.VoteValue==false).Count().ToString();
            upvoteCount = comment.VotingComment.Where(o => o.VoteValue).Count().ToString();
            return Json(upvoteCount + " : " + count);
        }

        [ChildActionOnly]
        public ActionResult NewsPartial(string source)
        {
            string downloadjson = "https://newsapi.org/v1/articles?source=" + source +
                            "&apiKey=346e17ce990f4aacac337fe81afb6f50";
            var json =
              c.DownloadString(downloadjson);
            Newsheadline newsheadline = Newtonsoft.Json.JsonConvert.DeserializeObject<Newsheadline>(json);
            var articles = newsheadline.Articles;
            return PartialView("_NewsPartial", articles);
        }
    }
}
