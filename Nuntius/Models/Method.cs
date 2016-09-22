using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Nuntius.Models
{
    public class DbActions
    {
        public void SaveToFavorite(Article xArticle, string source, string userId)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            xArticle.Source = context.Sources.FirstOrDefault(a => a.Id == source);
            xArticle.SourceId = source;

            var p = userId;
            var x = context.Users.FirstOrDefault(a => a.Id == p);
            Favourite favor = new Favourite();
            Favourite favourite = context.Favourits.FirstOrDefault(a => a.FavouriteId == x.FavouriteId);

            favourite.Articles.Add(xArticle);

            context.Entry(favourite).State = EntityState.Modified;
            context.SaveChanges();

        }
        public void RemoveFromFavorites(Article xArticle, string source, string userId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {


                //Finds the right article
                xArticle.Source = context.Sources.FirstOrDefault(a => a.Id == source);
                xArticle.SourceId = source;


                var foundUser = context.Users.FirstOrDefault(a => a.Id == userId);

                Favourite favourite = context.Favourits.FirstOrDefault(a => a.FavouriteId == foundUser.FavouriteId);

                favourite.Articles.Remove(xArticle);

                context.Entry(favourite).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}