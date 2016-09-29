function CountVote(id, userId) {
    var result = "";
    $.ajax({
        method: 'Post',
        url: '/Article/CountVote',
        async: false,
        data: {
            id: id,
            userId: userId
        },
        dataType: 'json',
        error: function (jqXHR, textStatus, errorThrown) {
            alert('Något gick fel! status:' + textStatus + "\nerror: " + errorThrown);
        },
        success: function (data) {
            result=data;

        }
    });
    return result;
};
function CommentUpvote(comment) {

    $.ajax({
        method: 'Post',
        url: '/Article/CommentUpVote',
        data: {
            id: comment.getAttribute("data-commentId"),
            userId: comment.getAttribute("data-userId")
        },
        dataType: 'json',
        error: function (jqXHR, textStatus, errorThrown) {
            alert('Något gick fel! status:' + textStatus + "\nerror: " + errorThrown);
        },
        success: function (data) {
            ShowComments();
        }
    });
};
function CommentDownvote(comment) {

    $.ajax({
        method: 'Post',
        url: '/Article/CommentDownVote',
        data: {
            id: comment.getAttribute("data-commentId"),
            userId: comment.getAttribute("data-userId")
        },
        dataType: 'json',
        error: function (jqXHR, textStatus, errorThrown) {
            alert('Något gick fel! status:' + textStatus + "\nerror: " + errorThrown);
        },
        success: function (data) {
            ShowComments();
        }
    });
};
function deleteComment(comment) {
    $.ajax({
        method: 'Post',
        url: '/Article/DeleteComment',
        data: {
            id: comment.getAttribute("data-commentId"),
            userId: comment.getAttribute("data-userId")
        },
        dataType: 'json',
        error: function (jqXHR, textStatus, errorThrown) {
            alert('Något gick fel! status:' + textStatus + "\nerror: " + errorThrown);
        },
        success: function (data) {
            if(data == "Deleted")
            $(comment).closest("li").remove();
        }
    });
}
function ShowComments() {
    $.ajax({
        method: 'Post',
        url: '/Article/ShowComments',
        data: {
            url: $('#url').val(),
        },
        dataType: 'json',
        error: function (jqXHR, textStatus, errorThrown) {
            alert('Något gick fel! status:' + textStatus + "\nerror: " + errorThrown);
        },
        success: function (data) {
            if (data === undefined) {
                alert('no data send');
            }
            else {
                $('#comments-list').empty();
                for (var i = 0; i < data.length; i++) {
                    var divcontent0 = CountVote(data[i].CommentId, data[i].ApplicationUserId);

                    var divcontent1 = '<li><div class="comment-main-level"><!-- Avatar --><div class="comment-avatar"><img src=' + data[i].User.ProfilePicture;
                    var divcontent2 = '></div><div class="comment-box"><div class="comment-head"><h6 class="comment-name by-author"><a href="#">' + data[i].User.UserName + '</a></h6>';
                    var divcontent3 = '<span>' + data[i].DatePublished + '</span>' +
                        '<i onclick="deleteComment(this)" data-userId = ' + data[i].ApplicationUserId + ' data-commentId=' + data[i].CommentId + ' class="glyphicon glyphicon-remove"></i> ' +
                        '<i onclick="CommentDownvote(this)" data-userId = ' + data[i].ApplicationUserId + ' data-commentId=' + data[i].CommentId + ' class="glyphicon glyphicon-arrow-down"></i> ' +
                        '<i>'+divcontent0 + '</i>'+
                        '<i onclick="CommentUpvote(this)" data-userId = ' + data[i].ApplicationUserId + ' data-commentId=' + data[i].CommentId + ' class="glyphicon glyphicon-arrow-up"></i> ' +
                        '</div><div class="comment-content">' +
                        data[i].CommentText + '</div></div></div></li>';
                    $('#comments-list').append(divcontent1 + divcontent2 + divcontent3);
                }


            }
            //$('#test').html("Letade du efter " + data.Name + "? id=" + data.Id);
        }
    });
}

function showtv(channel) {
    $.ajax({
        method: "GET",
        dataType: 'json',
        url: 'http://www.filmon.com/tv/api/init?app_id=7870Mo&app_secret=se8z1tt9',
        success: function (data) {
            if (data === undefined) {
                alert('no data send');
            }
            else {
                sessionk = data.session_key;
                var channel_id = 11;
                channelsurl = 'http://www.filmon.com/tv/api/channel/' + channel + '?app_id=7870Mo&app_secret=se8z1tt9&session_key=' + sessionk;
                var showData = $('#player');
                var showTitle = $('#titleid');
                var showDescription = $('#descriptionid');
                $.getJSON(channelsurl, function (data) {
                    showData.empty();
                    showTitle.empty();
                    showDescription.empty();
                    var divcontent1 = '<iframe class="single_iframe" scrolling="No" width="100%" height="460" src="' + 'http://afsoonplay.com/play/fplayer.php?id=' + data.streams[0].url + '" frameborder="0" allowfullscreen=""></iframe>'
                    showData.append(divcontent1);
                    showTitle.append(data.title);
                    showDescription.append(data.description)

                });
            }
        }
    });
}
$(document).ready(function () {
    $('#SendComment').on('click', function () {
        
        $.ajax({
            method: 'Post',
            url: '/Article/AddComment',
            data: {
                comment: $('#message-text').val(),
                urlId: $('#urlId').val(),
                articleSource: $('#articleSource').val()
            },
            dataType: 'json',
            error: function (jqXHR, textStatus, errorThrown) {
                alert('Något gick fel! status:' + textStatus + "\nerror: " + errorThrown);
            },
            success: function (data) {
                $('#message-text').val('');
                $('#showAlert').html( '<div class="alert alert-info">'+
                    '<a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>'+
                data+'</div>');
                ShowComments();
            }
        });
    });
    
    });