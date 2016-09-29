using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Nuntius.Models;

namespace Nuntius.Controllers
{
    public class FavoriteController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        // GET: Favorite
        public ActionResult Index()
        {

            var userId = User.Identity.GetUserId();
            var CurrentUser = context.Users.FirstOrDefault(a => a.Id == userId);
            Favourite currentFavouriteList =
                context.Favourits.FirstOrDefault(a => a.FavouriteId == CurrentUser.FavouriteId);


            return View(currentFavouriteList);
        }

        // GET: Favorite/Details/5
        public ActionResult Delete(int id)
        {
            var userId = User.Identity.GetUserId();
            var CurrentUser = context.Users.FirstOrDefault(a => a.Id == userId);
            Favourite currentFavouriteList =
                context.Favourits.FirstOrDefault(a => a.FavouriteId == CurrentUser.FavouriteId);

            var x = currentFavouriteList.Articles.FirstOrDefault(a => a.ArticleId == id);

            currentFavouriteList.Articles.Remove(x);

            context.Entry(currentFavouriteList).State = EntityState.Modified;
            context.SaveChanges();

            return Redirect(Request.UrlReferrer.PathAndQuery);
        }
    }
}
