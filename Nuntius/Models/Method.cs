using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace Nuntius.Models
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Splits an array into several smaller arrays.
        /// </summary>
        /// <typeparam name="T">The type of the array.</typeparam>
        /// <param name="array">The array to split.</param>
        /// <param name="size">The size of the smaller arrays.</param>
        /// <returns>An array containing smaller arrays.</returns>
        public static IEnumerable<IEnumerable<T>> Split<T>(this T[] array, int size)
        {
            for (var i = 0; i < (float)array.Length / size; i++)
            {
                yield return array.Skip(i * size).Take(size);
            }
        }
    }

    public class DbActions
    {

        public void SaveToFavorite(Article xArticle, string source, string userId)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            xArticle.Source = context.Sources.FirstOrDefault(a => a.Id == source);
            xArticle.SourceId = source;

            var p = userId;
            var userFavorite = context.Users.FirstOrDefault(a => a.Id == p);
            Favourite favor = new Favourite();
            Favourite favourite = context.Favourits.FirstOrDefault(a => a.FavouriteId == userFavorite.FavouriteId);
            var containsArticle = favourite.Articles.FirstOrDefault(a => a.Title == xArticle.Title);
            if (containsArticle == null)
            {
                favourite.Articles.Add(xArticle);
                context.Entry(favourite).State = EntityState.Modified;
            }




            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        var m = "Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage;
                    }
                }
            }

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
                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            var m = "Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage;
                        }
                    }
                }

            }
        }
    }

}
