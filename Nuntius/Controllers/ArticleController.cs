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
using System.Data.Entity;

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
            var json = c.DownloadString(downloadjson);
            Newsheadline newsheadline = Newtonsoft.Json.JsonConvert.DeserializeObject<Newsheadline>(json);
            var x = newsheadline.Articles.FirstOrDefault(a => helperfunctions.hashing(a.Title) == id);

            var sourcearticle = context.Sources.FirstOrDefault(a => a.Id == source);
            ArticleSource newssource = new ArticleSource()
            {
                Article = x,
                Source = sourcearticle,
                Articles = newsheadline.Articles
            };
		    var currentuser = User.Identity.GetUserId();
            @ViewBag.User = context.Users.FirstOrDefault(a => a.Id == currentuser);
            return View(newssource);
        }
        public ActionResult Saved(string id, string source)
        {

            string downloadjson = "https://newsapi.org/v1/articles?source=" + source +
                            "&apiKey=346e17ce990f4aacac337fe81afb6f50";
            var json = c.DownloadString(downloadjson);
            Newsheadline newsheadline = Newtonsoft.Json.JsonConvert.DeserializeObject<Newsheadline>(json);
            var x = context.Articles.ToList().FirstOrDefault( a => helperfunctions.hashing(a.Title) == id);

            var sourcearticle = context.Sources.FirstOrDefault(a => a.Id == source);
            ArticleSource newssource = new ArticleSource()
            {
                Article = x,
                Source = sourcearticle,
                Articles = newsheadline.Articles
            };
            var currentuser = User.Identity.GetUserId();
            @ViewBag.User = context.Users.FirstOrDefault(a => a.Id == currentuser);
            return View("Index",newssource);
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


            action.SaveToFavorite(xArticle, source, currentUserId);


            return Redirect(Request.UrlReferrer.PathAndQuery);
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
                    Article savedArticle;
                    if (!IsThereAnyArticle)
                    {

                        savedArticle = context.Articles.Add(article);
                        context.SaveChanges();
                    }
                    
                        savedArticle = context.Articles.Include("Comments").FirstOrDefault(o => o.Url == urlId);

                    savedArticle.Comments.Add(new Comment { CommentText = comment,  ArticleId = article.ArticleId, DatePublished = DateTime.Now, ApplicationUserId = currentUserId });
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
        [HttpPost]
        public ActionResult ArticleUpvote(string vote,string urlId, string articleSource)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            context.Configuration.ProxyCreationEnabled = false;
            int upvoteCount = 0;
            /*********************************/
            WebClient c = new WebClient();
            string downloadjson = "https://newsapi.org/v1/articles?source=" + articleSource +
                            "&apiKey=346e17ce990f4aacac337fe81afb6f50";
            string currentUserId = User.Identity.GetUserId();
            if (currentUserId != null)
            {
                var json =
                  c.DownloadString(downloadjson);
                ApplicationUser currentUser = context.Users.FirstOrDefault(o => o.Id == currentUserId);
                Newsheadline newsheadline = Newtonsoft.Json.JsonConvert.DeserializeObject<Newsheadline>(json);
                Article article = newsheadline.Articles.Where(o => o.Url == urlId).FirstOrDefault();
                //Check if the article is already in the database if not save it in database
                var IsThereAnyArticle = context.Articles.Any(o => o.Url == article.Url);
                article.Source = context.Sources.Where(o => o.Id == articleSource).FirstOrDefault();
                Article savedArticle;
                if (!IsThereAnyArticle)
                {
                    savedArticle = context.Articles.Add(article);
                    context.SaveChanges();
                    savedArticle = context.Articles.Include("VotingArticles").FirstOrDefault(o => o.Url == urlId);
                }
                else
                {
                    savedArticle = context.Articles.Include("VotingArticles").FirstOrDefault(o => o.Url == urlId);
                }
                var b =
                context.VotingArticles.Any(x => x.ApplicationUserID == currentUserId && x.Article.Url == urlId);
                if (!b)
                {
                    if (vote == "upvote")
                    {
                        savedArticle.VotingArticles.Add(new VotingArticle { ArticleId = article.ArticleId, VoteValue = true, ApplicationUserID = currentUserId });
                    }
                    else if (vote == "upvote")
                    {
                        savedArticle.VotingArticles.Add(new VotingArticle { ArticleId = article.ArticleId, VoteValue = false, ApplicationUserID = currentUserId });
                    }
                }
                else
                {
                    if (vote == "upvote")
                        savedArticle.VotingArticles.FirstOrDefault(x => x.ApplicationUserID == currentUserId && x.Article.Url == urlId).VoteValue = true;
                    if (vote == "downvote")
                        savedArticle.VotingArticles.FirstOrDefault(x => x.ApplicationUserID == currentUserId && x.Article.Url == urlId).VoteValue = false;
                }

                context.SaveChanges();
                upvoteCount = savedArticle.VotingArticles.Where(o => o.VoteValue).Count();
            }
            return Json("success");
        }
        public ActionResult CountArticleVote(string url)
        {
           
            ApplicationDbContext db = new ApplicationDbContext();
            db.Configuration.ProxyCreationEnabled = false;
            int upCount = 0;
            int downCount = 0;
            if (db.Articles.Any(o => o.Url == url))
            {
                var article = db.Articles.Include("VotingArticles").Where(o => o.Url == url).FirstOrDefault();
                 upCount = article.VotingArticles.Where(o => o.VoteValue).Count();
                 downCount = article.VotingArticles.Where(o => o.VoteValue == false).Count();
            }
            return Json(new int[] {upCount,downCount });
        }
    }
}
