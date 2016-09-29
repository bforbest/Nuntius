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

     
        [JsonProperty("author")]
        public string Author { get; set; }

       
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }


        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("urlToImage")]
        public string UrlToImage { get; set; }
        [JsonProperty("publishedAt")]
        public string PublishedAt { get; set; }
      
        public virtual IList<VotingArticle> VotingArticles { get; set; }
        public virtual IList<Comment> Comments { get; set; }

    
        public string SourceId { get; set; }
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
            public DateTime DatePublished { get; set; }
            public string ApplicationUserId { get; set; }
            public ApplicationUser User { get; set; }
            public int ArticleId { get; set; }
            public Article Article { get; set; }    
            public virtual IList<Comment> Comments { get; set; }
            public virtual IList<VotingComment> VotingComment { get; set; }
    }
        //ONE SUBSCRIPTION CAN CONTAIN MANY SOURCES - ADDED FOREIGN KEY TO USER ID SO IT'S LINKED WITH A USER.
        public class Subscription
        {
            [Key]
            public int SubscriptionId { get; set; }
            public string Title { get; set; }
            //public string ApplicationUserID { get; set; }
            //public virtual ApplicationUser ApplicationUser { get; set; }
            public virtual ICollection<Source> Sources { get; set; }
    }
        //ONE SOURCE CAN HAVE MANY CATEGORIES
        public class Source
        {
            public string SourceId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Url { get; set; }
            public string Category { get; set; }
            public string Language { get; set; }
            public string Country { get; set; }
            public string Id { get; set; }
           //public string Url { get; set; }
            public UrlsToLogos UrlsToLogos { get; set; }
            public List<string> SortBysAvailable { get; set; }
            public virtual ICollection<Subscription> Subscriptions { get; set; }

    }
        public class UrlsToLogos
        {
            public string Small { get; set; }
            public string Medium { get; set; }
            public string Large { get; set; }
        }
        public class AllSources
        {
            public string Status { get; set; }
            public List<Source> Sources { get; set; }
        }


        public class Favourite
            {
                public int FavouriteId { get; set; }
                public virtual IList<Article> Articles { get; set; }

        }
        public class VotingArticle
        {
            public int VotingArticleId { get; set; }
            public bool VoteValue { get; set; }
            public int ArticleId { get; set; }
            public virtual Article Article { get; set; }
            public string ApplicationUserID { get; set; }
            public virtual ApplicationUser ApplicationUser { get; set; }


        }
    public class VotingComment
    {
        public int VotingCommentId { get; set; }
        public int CommentId { get; set; }
        public bool VoteValue { get; set; }
        public virtual Comment Comment { get; set; }
        public string ApplicationUserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }


    }
}
