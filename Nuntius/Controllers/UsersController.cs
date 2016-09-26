using Nuntius.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Nuntius.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index(string id)
        {
            if (Request.IsAuthenticated && id == User.Identity.GetUserId())
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    try
                    {
                        var x = context.Users.FirstOrDefault(a => a.Id == id);
                        //ApplicationUser OwnUser =   context.Users.FirstOrDefault(a => a.Id == id);
                        //   return View("Index",OwnUser);
                        var currentUser = manager.FindById(User.Identity.GetUserId());
                        UserViewModel userViewmodel = new UserViewModel();
                        userViewmodel.Name = x.UserName;
                        userViewmodel.Comments = context.Comments.Where(a => a.User.Id == currentUser.Id).ToList();
                      
                        userViewmodel.ProfilePicture = x.ProfilePicture;
                        return View(userViewmodel);


                    }
                    catch
                    {


                    }
                }



            }
            else
            {

            }
            return View();
        }
        public PartialViewResult Load()
        {

            WebClient c = new WebClient();
            string downloadjson = "https://newsapi.org/v1/sources?language=en&apiKey=346e17ce990f4aacac337fe81afb6f50";
            var json =
                c.DownloadString(downloadjson);

            Source Sauce = Newtonsoft.Json.JsonConvert.DeserializeObject<Source>(json);
            AllSources RootObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AllSources>(json);
            Newsheadline newsheadline = Newtonsoft.Json.JsonConvert.DeserializeObject<Newsheadline>(json);
            var w = newsheadline;
            Article Art = new Article();
            var v = newsheadline;

            ApplicationDbContext context = new ApplicationDbContext();



           
            //foreach (var item in RootObject.sources)
            //{
            //    Source Saws = new Source();
            //    Saws.Id = item.Id;

            //    Saws.name = item.name;
            //    Saws.sortBysAvailable = item.sortBysAvailable;
            //    Saws.category = item.category;
            //    Saws.country = item.country;
            //    Saws.description = item.description;
            //    Saws.language = item.language;
            //    Saws.url = item.url;
            //    Saws.urlsToLogos = item.urlsToLogos;
            //    context.Sources.Add(Saws);
            //    context.SaveChanges();
            //};




            ViewBag.x = RootObject.Sources;
            var rootsources = RootObject;
            return PartialView("_Subscription", rootsources);
        }
        public ActionResult Edit(string id, FormCollection collection)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var url = Request["imgurl"];
                    var userId = User.Identity.GetUserId();
                    var user = context.Users.Find(userId);
                    try
                    {  //Om man har url bara så går den in här

                        user.ProfilePicture = url;
                        context.SaveChanges();

                        return RedirectToAction("Index" + "/" + userId, "User");
                    }
                    catch
                    {
                        ViewBag.Message = "You done fucked up";
                        return RedirectToAction("Index" + "/" + userId, "User");
                    }
                }

            }
            catch
            {
                return View();
            }
        }
    }
}
