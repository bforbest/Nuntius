function ArticleUpvote(article) {
    $.ajax({
        method: 'Post',
        url: '/Article/ArticleUpvote',
        data: {
            urlId: article.getAttribute("data-urlId"),
            articleSource: article.getAttribute("data-articlesource"),
            vote: article.getAttribute("data-vote")
        },
        dataType: 'json',
        error: function (jqXHR, textStatus, errorThrown) {
            alert('ArticleUpvote gick fel! status:' + textStatus + "\nerror: " + errorThrown);
        },
        success: function (data) {
            CountArticle(article.getAttribute("data-urlId"))
        }
    });
};

function CountArticle(artUrl) {
    $.ajax({
        method: 'Post',
        url: '/Article/CountArticleVote',
        data: {
            url: artUrl
        },
        dataType: 'json',
        error: function (jqXHR, textStatus, errorThrown) {
            alert('ArticleUpvote gick fel! status:' + textStatus + "\nerror: " + errorThrown);
        },
        success: function (data) {
            $('#upCount').html(data[0]);
            $('#downCount').html(data[1]);
        }
    });
};