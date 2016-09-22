using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Nuntius.Models;

namespace Nuntius.Controllers
{
    public class FavoriteController : Controller
    {
        // GET: Favorite
        public ActionResult Index()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var userId = User.Identity.GetUserId();
            var CurrentUser = context.Users.FirstOrDefault(a => a.Id == userId);
            Favourite currentFavouriteList =
                context.Favourits.FirstOrDefault(a => a.FavouriteId == CurrentUser.FavouriteId);


            return View(currentFavouriteList);
        }

        // GET: Favorite/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Favorite/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Favorite/Create
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

        // GET: Favorite/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Favorite/Edit/5
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

        // GET: Favorite/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Favorite/Delete/5
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
    }
}
