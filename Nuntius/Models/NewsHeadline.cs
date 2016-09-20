using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Nuntius.Models
{
        public class Newsheadline
        {
            [JsonProperty("status")]
            public string Status { get; set; }
            [JsonProperty("source")]
            public string Source { get; set; }
            [JsonProperty("sortBy")]
            public string SortBy { get; set; }
            [JsonProperty("articles")]
            public Article[] Articles { get; set; }
        }
        //JSON PROPERTIES ARE FOR THE API.
        //ONE ARTICLE CONTAINS LIST OF COMMENTS, LIST OF CATEGORIES, SOURCE ID AND/OR SOURCE, COULDNT DECIDE WHICH ONE
        public class Article
        {
            [Key]
            [Required]
            public int ArticleId { get; set; }
            [Required]
            [JsonProperty("author")]
            public string Author { get; set; }
            [Required]
            [JsonProperty("title")]
            public string Title { get; set; }
            [Required]
            [JsonProperty("description")]
            public string Description { get; set; }
            [Required]
            [JsonProperty("url")]
            public string Url { get; set; }
            [JsonProperty("urlToImage")]
            public string UrlToImage { get; set; }
            [Required]
            [JsonProperty("publishedAt")]
            public string PublishedAt { get; set; }
            public virtual IList<VotingArticle> VotingArticles { get; set; }
            public virtual IList<Comment> Comments { get; set; }
            [Required]
            public int? SourceId { get; set; }
            //[ForeignKey("SourceId")]
            public Source Source { get; set; }
        }
        //ONE COMMENT CONTAINS COMMENT ID, COMMENT TEXT, DATEPUBLISHED, USERID, ARTICLE/OR ARTICLE ID
        //NOT SURE IF TO USE ONLY ARTICLE ID OR ARTICLE TOO
        public class Comment
        {
            [Key]
            public int CommentId { get; set; }
            public string CommentText { get; set; }
            public int UpvoteComment { get; set; }
            public int DownvoteComment { get; set; }
            public DateTime DatePublished { get; set; }
            [ForeignKey("Id")]
            public ApplicationUser User { get; set; }
            public string Id { get; set; }
            [ForeignKey("ArticleId")]
            public Article Article { get; set; }
            public int ArticleId { get; set; }
            public virtual IList<Comment> Comments { get; set; }
        }
        //ONE SUBSCRIPTION CAN CONTAIN MANY SOURCES - ADDED FOREIGN KEY TO USER ID SO IT'S LINKED WITH A USER.
        public class Subscription
        {
            [Key]
            public int SubscriptionId { get; set; }
            public virtual ICollection<Source> Source { get; set; }
            [ForeignKey("Id")]
            public ApplicationUser User { get; set; }
            public string Id { get; set; }
        }
        //ONE SOURCE CAN HAVE MANY CATEGORIES
        public class Source
        {
            public string SourceId { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public string url { get; set; }
            public string category { get; set; }
            public string language { get; set; }
            public string country { get; set; }
            public UrlsToLogos urlsToLogos { get; set; }
            public List<string> sortBysAvailable { get; set; }
        }
        public class UrlsToLogos
        {
            public string small { get; set; }
            public string medium { get; set; }
            public string large { get; set; }
        }
        public class AllSources
        {
            public string status { get; set; }
            public List<Source> sources { get; set; }
        }


        public class Favourite
            {
                public int FavouriteId { get; set; }
                public virtual IList<Article> Articles { get; set; }

        }
        public class VotingArticle
        {
            public int VotingArticleId { get; set; }
            public int VoteValue { get; set; }
            public int ArticleId { get; set; }
            public virtual Article Article { get; set; }
            public string ApplicationUserID { get; set; }
            public virtual ApplicationUser ApplicationUser { get; set; }


        }
    public class VotingComment
    {
        public int VotingCommentId { get; set; }
        public virtual IList<Comment> Comments { get; set; }
        public string ApplicationUserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }


    }
}